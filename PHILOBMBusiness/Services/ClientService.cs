using AutoMapper;
using PHILOBMBusiness.Services.Interfaces;
using PHILOBMCore.Models;
using PHILOBMDatabase.Repositories.Interfaces;

namespace PHILOBMBusiness.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public ClientService(IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    public async Task AddAsync(Client entity)
    {
        var clientEntity = _mapper.Map<PHILOBMDatabase.Models.Client>(entity);
        await _clientRepository.AddAsync(clientEntity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _clientRepository.DeleteAsync(id);
    }

    public async Task<ICollection<Client>> GetAllAsync()
    {
        var clients = await _clientRepository.GetAllAsync();
        return _mapper.Map<ICollection<Client>>(clients);
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        var client = await _clientRepository.GetByIdAsync(id);
        return _mapper.Map<Client?>(client);
    }

    public async Task<Client?> GetClientByIdWithCarsAsync(int clientId)
    {
        var clientWithCars = await _clientRepository.GetClientByIdWithCarsAsync(clientId);
        return _mapper.Map<Client?>(clientWithCars);
    }

    public async Task UpdateAsync(Client entity)
    {
        var clientEntity = _mapper.Map<PHILOBMDatabase.Models.Client>(entity);
        await _clientRepository.UpdateAsync(clientEntity);
    }

    public async Task<PHILOBMDatabase.Models.DatabaseData> ExportDatabaseToJson()
    {
        return await _clientRepository.ExportDatabaseToJson();
    }
}
