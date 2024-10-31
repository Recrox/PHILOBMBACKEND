using Microsoft.EntityFrameworkCore;
using PHILOBMDatabase.Models.Base;
using PHILOBMDatabase.Repositories.Interfaces;
using PHILOBMDatabase.Database;
using AutoMapper;

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

    public async Task<ICollection<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

    public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FirstOrDefaultAsync(item => item.Id == id);

    public async Task AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

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

}
