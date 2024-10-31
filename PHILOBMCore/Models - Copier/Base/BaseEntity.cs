using System.ComponentModel.DataAnnotations;

namespace PHILOBMCore.Models.Base;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
