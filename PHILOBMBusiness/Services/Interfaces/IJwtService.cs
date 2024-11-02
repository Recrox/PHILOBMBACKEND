using System.Security.Claims;

namespace PHILOBMBusiness.Services.Interfaces;

public interface IJwtService
{
    public string GenerateToken(ClaimsIdentity identity);
}