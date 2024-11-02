using Configuration;
using Jose;
using Microsoft.IdentityModel.Tokens;
using PHILOBMBusiness.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PHILOBMBusiness.Services;

public class JwtService : IJwtService
{
    private readonly ConfigurationSettings _configurationSettings;

    public JwtService(ConfigurationSettings  configurationSettings)
    {
        _configurationSettings = configurationSettings;
    }

    public string GenerateToken(ClaimsIdentity identity)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configurationSettings.Jwt.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddMinutes(_configurationSettings.Jwt.ExpirationInMinutes),
            Issuer = _configurationSettings.Jwt.Issuer,
            Audience = _configurationSettings.Jwt.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}