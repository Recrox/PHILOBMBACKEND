using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PHILOBMDatabase.Database;
using PHILOBMDatabase.Repositories.Interfaces;
using PHILOBMCore.Models;
namespace PHILOBMDatabase.Repositories;

public class ClientRepository : BaseRepository<Client,Models.Client>, IClientRepository
{

    public ClientRepository(PhiloBMContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public async Task<Client?> GetClientByIdWithCarsAsync(int clientId)
    {
        var clientEntity = await _context.Clients
            .Include(client => client.Cars) // Inclure les voitures
            .FirstOrDefaultAsync(client => client.Id == clientId);

        return clientEntity != null ? _mapper.Map<Models.Client, Client>(clientEntity) : null;
    }
}
