using Catalogue.Domain.Abstractions;
using Catalogue.Domain.Repositories;
using Catalogue.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Catalogue.Infrastructure.CommandRepo;

public class CommandRepository<T> : ICommandRepository<T> where T : AggregateRoot
{
    protected readonly CatalogueDbContext _context;
    public IUnitOfWork UnitOfWork => _context;
    protected DbSet<T> entities;
    public CommandRepository(CatalogueDbContext context)
    {
        _context = context;
        entities = context.Set<T>();
    }

    public async Task<T> Get(long id) => await entities.FindAsync(id);
    public async Task<T> Get(Expression<Func<T, bool>> predicate) => await entities.FindAsync(predicate);

    public T Add(T entity) => entities.Add(entity).Entity;
    public void AddRange(List<T> entities) => entities.AddRange(entities);
    public void Delete(T entity) => entities.Remove(entity);

    public async Task<int> Count(Expression<Func<T, bool>> predicate) => await entities.CountAsync(predicate);

}

