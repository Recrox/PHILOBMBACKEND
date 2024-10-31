using PHILOBMDatabase.Models.Base;

namespace PHILOBMDatabase.Models;

public class Service : AuditableEntity
{
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Units { get; set; }
    public int? CarId { get; set; } // Clé étrangère
    public Car? Car { get; set; } // Référence à la voiture associée

    public decimal CalculateCost()
    {
        return Price;
    }
}
