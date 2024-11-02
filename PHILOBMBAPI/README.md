# PHILOBMBAPI

update la db:
dotnet ef database update

delete la derniere migration:
dotnet ef migrations remove

voir toutes les migrations:
dotnet ef migrations list

ajouter new migration:
dotnet ef migrations add InitialCreate

rollback � une ancienne migration:
dotnet ef database update LastGoodMigration

delete une vieille migration:
DELETE FROM "__EFMigrationsHistory" WHERE "MigrationId" = 'YourMigrationName';

dotnet ef database drop

ajouter dans la db les diff�rentes migration:
INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) 
VALUES ('20241101144947_date_in_service', '8.0.0'); 


lancer le .bat
installer derni�re version de dotnet
https://dotnet.microsoft.com/fr-fr/download/dotnet

Solution 3: Script SQL pour la synchronisation
Si vous avez des migrations manquantes uniquement dans la base de donn�es, vous pouvez ex�cuter une migration pour g�n�rer un script SQL qui met � jour la base de donn�es � l��tat actuel d�fini par vos fichiers de migration.

G�n�rez un script de migration :
bash
dotnet ef migrations script > sync_migrations.sql
Ex�cutez le script dans votre base de donn�es.
Ce script devrait mettre � jour la base de donn�es pour qu'elle corresponde aux migrations existantes dans le dossier Migrations.


Solution 4: Utiliser dotnet ef migrations bundle (EF Core 7+)
Avec EF Core 7, vous pouvez cr�er un ex�cutable pour appliquer toutes les migrations jusqu'� la derni�re, ce qui est pratique pour les environnements o� vous ne pouvez pas utiliser directement dotnet ef.
dotnet ef migrations bundle
