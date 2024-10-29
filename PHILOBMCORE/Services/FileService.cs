using System.IO;
using System.Windows;

namespace PHILOBMCore.Services;

public class FileService
{
    public string DatabaseFileName { get; set; } = null!;
    public string BackupDirectory { get; set; } = null!;
    public int MaxBackupCount { get; set; } 
    public bool ShowMessageBoxes { get; set; }

    public void SauvegarderBaseDeDonnees()
    {
        try
        {
            // Créer le dossier de sauvegarde s'il n'existe pas
            if (!Directory.Exists(BackupDirectory))
            {
                Directory.CreateDirectory(BackupDirectory);
            }

            // Chemin complet de la base de données actuelle
            string sourceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFileName);

            // Créer un nom de fichier de sauvegarde unique
            string backupFileName = Path.Combine(BackupDirectory, $"{DatabaseFileName}_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

            // Copier le fichier de base de données
            File.Copy(sourceFile, backupFileName, true);

            // Gérer les sauvegardes excédentaires
            GérerSauvegardesExcédentaires();

            // Afficher le MessageBox si l'option est activée
            if (ShowMessageBoxes)
            {
                //MessageBox.Show($"Sauvegarde réussie : {backupFileName}", "Sauvegarde", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show($"Erreur lors de la sauvegarde de la base de données : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void GérerSauvegardesExcédentaires()
    {
        var backupFiles = Directory.GetFiles(BackupDirectory)
            .OrderBy(f => f)
            .ToList();

        if (backupFiles.Count > MaxBackupCount)
        {
            int filesToDelete = backupFiles.Count - MaxBackupCount;
            for (int i = 0; i < filesToDelete; i++)
            {
                File.Delete(backupFiles[i]);
            }

            if (ShowMessageBoxes)
            {
                //MessageBox.Show($"{filesToDelete} sauvegardes supprimées pour maintenir le nombre à {MaxBackupCount}.", "Gestion des Sauvegardes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
