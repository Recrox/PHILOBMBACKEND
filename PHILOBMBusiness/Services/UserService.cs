using Microsoft.EntityFrameworkCore;
using PHILOBMBusiness.Services.Interfaces;
using PHILOBMCore.Models;
using PHILOBMDatabase.Repositories.Interfaces;

namespace PHILOBMBusiness.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository )
    {
        _userRepository = userRepository;
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        // Remplacez par la logique de vérification des identifiants dans votre base de données
        var user = await _userRepository.AuthenticateAsync(username, password);

        return user; // Null si non trouvé, sinon retourne l'utilisateur
    }
}