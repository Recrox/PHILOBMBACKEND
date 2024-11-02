namespace Configuration;

public class JwtSettings
{
    public string Secret { get; set; } // Utiliser une chaîne pour plus de simplicité
    public int ExpirationInMinutes { get; set; } // Utiliser int pour la durée en minutes
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
