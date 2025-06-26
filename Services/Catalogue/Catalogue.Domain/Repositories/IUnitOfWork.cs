
using Microsoft.EntityFrameworkCore.Storage;

namespace Catalogue.Domain.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
    IDbContextTransaction GetCurrentTransaction();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync(IDbContextTransaction transaction);
    void RollbackTransaction();
}

