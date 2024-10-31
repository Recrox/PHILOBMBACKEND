namespace PHILOBMBusiness.Services.Interfaces;

public interface IBaseService<T>
{
    Task<ICollection<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
}