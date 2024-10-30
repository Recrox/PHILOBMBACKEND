# PHILOBMBAPI

update la db:
dotnet ef database update

delete la derniere migration:
dotnet ef migrations remove

voir toutes les migrations:
dotnet ef migrations list

ajouter new migration:
dotnet ef migrations add InitialCreate

rollback à une ancienne migration:
dotnet ef database update LastGoodMigration

delete une vieille migration:
DELETE FROM "__EFMigrationsHistory" WHERE "MigrationId" = 'YourMigrationName';

dotnet ef database drop


