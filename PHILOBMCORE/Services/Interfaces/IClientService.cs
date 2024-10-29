using PHILOBMCore.Models;
namespace PHILOBMCore.Services.Interfaces;

public interface IClientService : IBaseContextService<Client>
{
    Task<Client?> GetClientByIdWithCarsAsync(int clientId);
    public bool IsClientValid();
}
