namespace PHILOBMDatabase.Repositories.Interfaces;

public interface IBaseRepository<T>
{
    Task<ICollection<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
}