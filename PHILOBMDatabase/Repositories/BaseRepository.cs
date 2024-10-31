using Microsoft.EntityFrameworkCore;
using PHILOBMDatabase.Models.Base;
using PHILOBMDatabase.Repositories.Interfaces;
using PHILOBMDatabase.Database;
using AutoMapper;
using System.Linq.Expressions;

namespace PHILOBMDatabase.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly PhiloBMContext _context;
    protected readonly IMapper _mapper;

    protected BaseRepository(PhiloBMContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Récupère tous les éléments
    public async Task<ICollection<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

    // Récupère un élément par ID
    public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FirstOrDefaultAsync(item => item.Id == id);

    // Ajoute un nouvel élément
    public async Task AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
    }

    // Met à jour un élément existant
    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    // Supprime un élément par ID
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true; // Renvoie vrai si la suppression a réussi
        }
        return false; // Renvoie faux si l'entité n'existe pas
    }

    // Compte le nombre total d'éléments
    public async Task<int> CountAsync()
    {
        return await _context.Set<T>().CountAsync();
    }

    // Récupère les éléments avec pagination
    public async Task<ICollection<T>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _context.Set<T>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    // Récupère les éléments en fonction d'une condition (filtrage dynamique)
    public async Task<ICollection<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    // Vérifie si un élément existe en fonction d'une condition
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().AnyAsync(predicate);
    }

    // Récupère les éléments avec tri
    public async Task<ICollection<T>> GetAllSortedAsync<TKey>(Expression<Func<T, TKey>> orderBy, bool ascending = true)
    {
        var query = _context.Set<T>().AsQueryable();
        query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        return await query.ToListAsync();
    }

    // Ajoute une liste d'éléments (insertion en lot)
    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    // Met à jour une liste d'éléments (mise à jour en lot)
    public async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        _context.Set<T>().UpdateRange(entities);
        await _context.SaveChangesAsync();
    }

    // Exécute une opération dans une transaction
    public async Task ExecuteInTransactionAsync(Func<Task> operation)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await operation();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
