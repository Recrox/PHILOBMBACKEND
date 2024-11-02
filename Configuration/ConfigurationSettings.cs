namespace Configuration;

public class ConfigurationSettings
{
    public JwtSettings Jwt { get; set; }

    public string DatabaseFileName { get; set; }
    public string BackupDirectory { get; set; }
    public int MaxBackupCount { get; set; }
    public bool ShowMessageBoxes { get; set; }
}
