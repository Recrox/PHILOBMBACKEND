using PHILOBMCore.Models;
namespace PHILOBMDatabase.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> AuthenticateAsync(string username, string password);
}