using System.Diagnostics;

namespace PHILOBMBusiness.Services;

public static class Outils
{
    /// <summary>
    /// Crée un dossier si il n'existe pas déjà.
    /// </summary>
    /// <param name="path">Le chemin du dossier à créer.</param>
    public static void CréerDossierSiInexistant(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public static void OpenFolder(string downloadFolderPath)
    {
        if (Directory.Exists(downloadFolderPath))
        {
            // Ouvre le dossier dans l'explorateur de fichiers
            Process.Start("explorer.exe", downloadFolderPath);
        }
        else
        {
            //MessageBox.Show($"Le dossier n'existe pas. à l'emplacment {downloadFolderPath}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

