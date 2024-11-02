using PHILOBMDatabase.Models.Base;

namespace PHILOBMDatabase.Models;

public class User : AuditableEntity
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // Assurez-vous que cette propriété est bien définie
}
