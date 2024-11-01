using System.Linq.Expressions;

namespace PHILOBMDatabase.Repositories.Interfaces;

public interface IBaseRepository<T>
{
    Task<ICollection<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
    Task<int> CountAsync();
    Task<ICollection<T>> GetPagedAsync(int pageNumber, int pageSize);


    Task<ICollection<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<ICollection<T>> GetAllSortedAsync<TKey>(Expression<Func<T, TKey>> orderBy, bool ascending = true);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateRangeAsync(IEnumerable<T> entities);
    Task ExecuteInTransactionAsync(Func<Task> operation);

    Task<Models.DatabaseData> ExportDatabaseToJson();
}
