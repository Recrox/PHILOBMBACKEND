using PHILOBMCore.Models;

namespace PHILOBMBusiness.Services.Interfaces;

public interface IClientService : IBaseService<Client>
{
    Task<Client?> GetClientByIdWithCarsAsync(int clientId);
}
