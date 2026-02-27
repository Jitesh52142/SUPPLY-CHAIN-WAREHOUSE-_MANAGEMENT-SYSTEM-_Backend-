using System.Linq.Expressions;
using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Infrastructure.Persistence.Repository;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}