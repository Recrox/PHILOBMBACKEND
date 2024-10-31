using PHILOBMBusiness.Services.Interfaces;
using PHILOBMCore.Models;
using PHILOBMDatabase.Repositories.Interfaces;

namespace PHILOBMBusiness.Services;

public class ClientService : BaseService<Client>, IClientService
{
    private readonly IClientRepository _clientRepository;
    public ClientService(IClientRepository clientRepository) : base(clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Client?> GetClientByIdWithCarsAsync(int clientId)
    {
        return await _clientRepository.GetClientByIdWithCarsAsync(clientId);
    }
}
