using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using warehouse_management_system.Database;
using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Infrastructure.Persistence.Repository;

public class Repository<T> : IRepository<T>
    where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.Where(x => !x.IsDeleted).ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id)
        => await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Delete(T entity)
        => entity.IsDeleted = true;

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();
}