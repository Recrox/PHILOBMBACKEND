﻿using PHILOBMCore.Models.Base;
using PHILOBMDatabase.Repositories.Interfaces;

namespace PHILOBMBusiness.Services;

public abstract class BaseService<T> where T : BaseEntity
{
    protected readonly IBaseRepository<T> _repository;

    protected BaseService(IBaseRepository<T> repository)
    {
        _repository = repository;
    }

    // Récupérer tous les éléments
    public async Task<ICollection<T>> GetAllAsync() => await _repository.GetAllAsync();

    // Récupérer un élément par ID
    public async Task<T?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    // Ajouter un nouvel élément
    public async Task AddAsync(T entity) => await _repository.AddAsync(entity);

    // Mettre à jour un élément existant
    public async Task UpdateAsync(T entity) => await _repository.UpdateAsync(entity);

    // Supprimer un élément par ID
    public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
}