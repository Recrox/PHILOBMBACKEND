using PHILOBMDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace PHILOBMDatabase.Database;

public class PhiloBMContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Service> Services { get; set; }


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


    //public override int SaveChanges()
    //{
    //    var entries = ChangeTracker.Entries()
    //        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

    //    foreach (var entry in entries)
    //    {
    //        if (entry.Entity is AuditableEntity auditableEntity)
    //        {
    //            var now = DateTime.UtcNow;
    //            auditableEntity.ModifiedDate = now;
    //            auditableEntity.ModifiedBy = "CurrentUser"; // Remplacez par l'utilisateur actuel si nécessaire

    //            // Créez une copie pour l'historique
    //            if (entry.Entity is Client client)
    //            {
    //                var history = new Client
    //                {
    //                    Id = client.Id,
    //                    LastName = client.LastName,
    //                    FirstName = client.FirstName,
    //                    Address = client.Address,
    //                    Phone = client.Phone,
    //                    Email = client.Email,

    //                    CreatedBy = client.CreatedBy,
    //                    CreatedDate = client.CreatedDate,
    //                    ModifiedBy = client.ModifiedBy,
    //                    ModifiedDate = client.ModifiedDate
    //                };

    //                // Ajout dans la table d'historique
    //                Set<Client>().FromSqlRaw("INSERT INTO ClientHistories SELECT * FROM {0}", history);
    //            }
    //            else if (entry.Entity is Car car)
    //            {
    //                var history = new Car
    //                {
    //                    Id = car.Id,
    //                    Model = car.Model,

    //                    CreatedBy = car.CreatedBy,
    //                    CreatedDate = car.CreatedDate,
    //                    ModifiedBy = car.ModifiedBy,
    //                    ModifiedDate = car.ModifiedDate
    //                };

    //                // Ajout dans la table d'historique
    //                Set<Car>().FromSqlRaw("INSERT INTO CarHistories SELECT * FROM {0}", history);
    //            }
    //        }
    //    }

    //    return base.SaveChanges();
    //}


}

