namespace PHILOBMDatabase.Repositories.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBaseRepository<TCore>
{
    Task<ICollection<TCore>> GetAllAsync(); // Récupérer tous les objets du modèle de domaine
    Task<TCore?> GetByIdAsync(int id); // Récupérer un objet par son ID
    Task AddAsync(TCore entity); // Ajouter un nouvel objet
    Task UpdateAsync(TCore entity); // Mettre à jour un objet existant
    Task<bool> DeleteAsync(int id); // Supprimer un objet par son ID
    Task<int> CountAsync(); // Compter le nombre total d'entités
}
