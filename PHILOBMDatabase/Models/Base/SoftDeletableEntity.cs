namespace PHILOBMDatabase.Models.Base;

public abstract class SoftDeletableEntity : AuditableEntity
{
    public string? DeletedBy { get; set; } // Utilisateur qui a supprimé l'entité
    public DateTime? DeletedOn { get; set; } // Date de suppression
}
