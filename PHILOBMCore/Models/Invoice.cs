using PHILOBMCore.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace PHILOBMCore.Models;

public class Invoice : AuditableEntity
{
    public Client Client { get; set; } = null!;
    public Car Car { get; set; } = null!;
    public DateTime Date { get; set; }
    public List<Service> Services { get; set; } = new List<Service>();

    [NotMapped]
    public decimal Total => CalculSum();
    public decimal CalculSum()
    {
        return Services.Sum(service => service.CalculateCost());
    }
}

