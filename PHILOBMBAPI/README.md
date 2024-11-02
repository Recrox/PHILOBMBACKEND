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

ajouter dans la db les différentes migration:
INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) 
VALUES ('20241101144947_date_in_service', '8.0.0'); 


lancer le .bat
installer dernière version de dotnet
https://dotnet.microsoft.com/fr-fr/download/dotnet

Solution 3: Script SQL pour la synchronisation
Si vous avez des migrations manquantes uniquement dans la base de données, vous pouvez exécuter une migration pour générer un script SQL qui met à jour la base de données à l’état actuel défini par vos fichiers de migration.

Générez un script de migration :
bash
dotnet ef migrations script > sync_migrations.sql
Exécutez le script dans votre base de données.
Ce script devrait mettre à jour la base de données pour qu'elle corresponde aux migrations existantes dans le dossier Migrations.


Solution 4: Utiliser dotnet ef migrations bundle (EF Core 7+)
Avec EF Core 7, vous pouvez créer un exécutable pour appliquer toutes les migrations jusqu'à la dernière, ce qui est pratique pour les environnements où vous ne pouvez pas utiliser directement dotnet ef.
dotnet ef migrations bundle
