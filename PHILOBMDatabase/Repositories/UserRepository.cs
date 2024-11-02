using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PHILOBMCore.Models;
using PHILOBMDatabase.Database;
using PHILOBMDatabase.Repositories.Interfaces;
namespace PHILOBMDatabase.Repositories;

public class UserRepository : BaseRepository<Models.User>, IUserRepository
{
    public UserRepository(PhiloBMContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        // Remplacez par la logique de vérification des identifiants dans votre base de données
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

        return user == null ? null : _mapper.Map<User>(user); // Null si non trouvé, sinon retourne l'utilisateur
    }
}
