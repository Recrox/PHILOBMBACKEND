using PHILOBMCore.Models;

namespace PHILOBMDatabase.Repositories.Interfaces;

public interface IClientRepository : IBaseRepository<Models.Client>
{
    Task<Client?> GetClientByIdWithCarsAsync(int clientId);
}
