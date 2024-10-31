using System.ComponentModel.DataAnnotations;

namespace PHILOBMDatabase.Models.Base;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
