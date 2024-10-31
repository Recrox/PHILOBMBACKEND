using PHILOBMDatabase.Models.Base;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PHILOBMDatabase.Models;

public class Client : AuditableEntity
{
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public ObservableCollection<Car> Cars { get; set; } = new ObservableCollection<Car>();

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
