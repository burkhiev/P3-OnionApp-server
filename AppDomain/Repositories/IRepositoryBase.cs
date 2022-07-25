using AppDomain.Entities;
using System.Linq.Expressions;

namespace AppDomain.Repositories
{
    public interface IRepositoryBase<T>
        where T : EntityBase, new()
    {
        IQueryable<T> GetAll();

        Task<T?> FindByIdAsync(Guid entityId, CancellationToken cancellationToken = default);

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        T Update(T entity);

        T Remove(T entity);
        void RemoveRange(T[] entities);
    }
}
