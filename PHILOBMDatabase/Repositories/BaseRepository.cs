using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PHILOBMCore.Models.Base;
using PHILOBMDatabase.Database;
using PHILOBMDatabase.Repositories.Interfaces;

public abstract class BaseRepository<TCore, TEntity> : IBaseRepository<TCore>
    where TCore : BaseEntity // TCore représente le modèle du domaine
    where TEntity : PHILOBMDatabase.Models.Base.BaseEntity // TEntity représente l'entité de base de données
{
    protected readonly PhiloBMContext _context;
    protected readonly IMapper _mapper;

    protected BaseRepository(PhiloBMContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ICollection<TCore>> GetAllAsync() =>
        await _mapper.ProjectTo<TCore>(_context.Set<TEntity>()).ToListAsync();

    public async Task<TCore?> GetByIdAsync(int id) =>
        _mapper.Map<TCore>(await _context.Set<TEntity>().FirstOrDefaultAsync(item => item.Id == id));

    public async Task AddAsync(TCore entity)
    {
        var entityToAdd = _mapper.Map<TEntity>(entity); // Mapper Core vers DB
        _context.Set<TEntity>().Add(entityToAdd);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TCore entity)
    {
        var entityToUpdate = _mapper.Map<TEntity>(entity); // Mapper Core vers DB
        _context.Set<TEntity>().Update(entityToUpdate);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return true; // Renvoie vrai si la suppression a réussi
        }
        return false; // Renvoie faux si l'entité n'existe pas
    }

    public async Task<int> CountAsync() =>
        await _context.Set<TEntity>().CountAsync(); // Compte le nombre d'entités
}
