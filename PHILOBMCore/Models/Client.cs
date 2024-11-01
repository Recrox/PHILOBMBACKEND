using PHILOBMCore.Models.Base;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PHILOBMCore.Models;

public class Client : AuditableEntity
{
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public ICollection<Car> Cars { get; set; } = new List<Car>();

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
