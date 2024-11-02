using PHILOBMCore.Models;

namespace PHILOBMBusiness.Services.Interfaces;

public interface IUserService
{
    Task<User?> AuthenticateAsync(string username, string password);
}
