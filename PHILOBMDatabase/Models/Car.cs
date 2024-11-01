using PHILOBMDatabase.Models.Base;

namespace PHILOBMDatabase.Models;

public class Car : AuditableEntity
{
    public string? Brand { get; set; } // Car brand
    public string? Model { get; set; } // Car model
    public string? LicensePlate { get; set; } // License plate number
    public string? ChassisNumber { get; set; } // Chassis number
    public int? Mileage { get; set; } // Mileage
    
    public ICollection<Service> Services { get; set; } = new List<Service>();

    public int? ClientId { get; set; } // Foreign key to Client
    public Client? Client { get; set; } // Owner
}



