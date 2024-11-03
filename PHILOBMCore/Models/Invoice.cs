using PHILOBMCore.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PHILOBMCore.Models;

public class Invoice : AuditableEntity
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public int CarId { get; set; }
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

