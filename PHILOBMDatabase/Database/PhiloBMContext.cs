using PHILOBMDatabase.Models;
using Microsoft.EntityFrameworkCore;
using PHILOBMDatabase.Models.Base;

namespace PHILOBMDatabase.Database;

public class PhiloBMContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<User> Users { get; set; }

    // Remplacez par le nom d'utilisateur actuel si disponible dans le contexte de l'application.
    private readonly string _currentUser = "System"; // Exemple, utilisez une méthode pour récupérer l'utilisateur actuel.


    public PhiloBMContext(DbContextOptions<PhiloBMContext> options)
           : base(options)
    {
        // Assurez-vous que la base de données est créée si elle n'existe pas
        this.Database.EnsureCreated();
        //this.Database.EnsureDeleted();
        //this.Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlite($"Data Source={ConstantsSettings.DBName}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasOne(c => c.Client)
            .WithMany(c => c.Cars)
            .HasForeignKey(c => c.ClientId)
            .OnDelete(DeleteBehavior.SetNull);



        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditFields()
    {
        var entries = ChangeTracker.Entries<AuditableEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

        foreach (var entry in entries)
        {
            var entity = entry.Entity;

            switch (entry.State)
            {
                case EntityState.Added:
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.CreatedBy = _currentUser;
                    break;

                case EntityState.Modified:
                    entity.ModifiedDate = DateTime.UtcNow;
                    entity.ModifiedBy = _currentUser;
                    break;

                case EntityState.Deleted when entity is SoftDeletableEntity softDeletableEntity:
                    softDeletableEntity.DeletedOn = DateTime.UtcNow;
                    softDeletableEntity.DeletedBy = _currentUser;
                    entry.State = EntityState.Modified; // Marquer comme modifié pour un soft delete
                    break;
            }
        }
    }

}

