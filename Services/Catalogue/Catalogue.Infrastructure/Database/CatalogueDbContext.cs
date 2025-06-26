using Catalogue.Domain.Abstractions;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

namespace Catalogue.Infrastructure.Database;

public class CatalogueDbContext : DbContext, IUnitOfWork
{

    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private IDbContextTransaction _currentTransaction;

    public int TenantId { get; }
    public int UserId { get; }
    public string UserName { get; }

    public CatalogueDbContext(DbContextOptions<CatalogueDbContext> options, IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(options)
    {

        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException();

        if (_httpContextAccessor.HttpContext != null)
        {
            TenantId = Convert.ToInt32(_httpContextAccessor.HttpContext.Items["TenantId"]);
            UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.Items["userId"]);
            UserName = Convert.ToString(_httpContextAccessor.HttpContext.Items["userName"]);
        }
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        await _mediator.DispatchPrePersistenceDomainEventsAsync(this);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed           
        await SaveChangesAsync(cancellationToken);

        await _mediator.DispatchPostPersistenceDomainEventsAsync(this);

        return true;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateClientAndAuditInfo();
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<DomainEventBase>();
        builder.Entity<Product>().HasQueryFilter(c => c.TenantId == TenantId);
       
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync();

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }

        }
    }

    private void UpdateClientAndAuditInfo()
    {
        ChangeStateOfOwnerEntity();
        var entities = base.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

        foreach (var entity in entities)
        {
            var e = entity.Entity;
            e.TenantId = TenantId;
            switch (entity.State)
            {
                case EntityState.Added:
                    e.AddedBy = UserId;
                    e.AddedByName = UserName;
                    e.AddedOnDate = DateTime.Now;
                    break;
                case EntityState.Modified:
                    e.EditedBy = UserId;
                    e.EditedByName = UserName;
                    e.EditedOnDate = DateTime.Now;
                    break;
            }
        }

        void ChangeStateOfOwnerEntity()
        {
            var allValueObjectEntries = base.ChangeTracker.Entries<ValueObject>()
                       .Where(e => e.State == EntityState.Added && (e.Metadata.FindOwnership()?.PrincipalEntityType.ClrType.BaseType == typeof(Entity) || e.Metadata.FindOwnership()?.PrincipalEntityType.ClrType.BaseType == typeof(AggregateRoot)));

            foreach (var entry in allValueObjectEntries)
            {
                var ownership = entry.Metadata.FindOwnership();
                var parentKey = ownership.Properties
                                 .Select(p => entry.Property(p.Name).CurrentValue).ToArray();

                var parent = this.Find(ownership.PrincipalEntityType.ClrType, parentKey);
                if (parent is null)
                    continue;

                var parentEntry = this.Entry(parent);
                if (parentEntry.State != EntityState.Added && parentEntry.State != EntityState.Modified)
                    parentEntry.State = EntityState.Modified;
            }
        }
    }
}
