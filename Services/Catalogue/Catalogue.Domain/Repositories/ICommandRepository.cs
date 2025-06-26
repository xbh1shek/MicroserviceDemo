
using Catalogue.Domain.Abstractions;
using System.Linq.Expressions;

namespace Catalogue.Domain.Repositories;

public interface ICommandRepository<T> where T : AggregateRoot
{
    IUnitOfWork UnitOfWork { get; }

    T Add(T entity);
    void AddRange(List<T> entities);
    void Delete(T entity);
    Task<T> Get(long id);
    Task<int> Count(Expression<Func<T, bool>> predicate);
    Task<T> Get(Expression<Func<T, bool>> predicate);
}

