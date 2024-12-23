﻿

namespace Configuration.ConstantsSettings;

public static class Constants
{
    public const string BackupFolderName = "Backups";
    public const string DBName = "philoBM.db";
    public const int MaxBackupCount = 100;
    public const bool ShowMessageBoxes = false;
    public const string MimeTypePDF = "application/pdf";
    public static readonly string DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
    public static readonly string DbPath = Path.Combine(RacinePath, DBName); // Chemin complet vers la DB



    //public static readonly string RacinePath = Path.Combine("C:", "PhiloBM");
    public static string RacinePath => Path.Combine(Path.GetPathRoot(AppContext.BaseDirectory) ?? "C:\\", "PhiloBM");
}
