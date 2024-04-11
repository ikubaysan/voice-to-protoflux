using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

public class ConfigManager
{
    private const string ConfigFileName = "appsettings.json";
    public string ApiKey { get; set; } = "YourApiKey";
    public string Region { get; set; } = "YourServiceRegion";
    public bool TerminalMessageBoxNeeded { get; set; } = false;
    public string TerminalMessageBoxText { get; set; } = "Critical Error";

    public void LoadConfig()
    {
        if (!File.Exists(ConfigFileName))
        {
            SaveDefaultConfig();
            string fullPath = Path.GetFullPath(ConfigFileName);
            //MessageBox.Show($"Config file created at {fullPath}. Please fill out the config file and then run the program again.", "Config Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //Application.Exit();
            TerminalMessageBoxNeeded = true;
            TerminalMessageBoxText = $"Config file created at {fullPath}. Please fill out the config file and then run the program again.";
        }
        else
        {
            try
            {
                string jsonString = File.ReadAllText(ConfigFileName);
                var config = JsonSerializer.Deserialize<ConfigManager>(jsonString);
                this.ApiKey = config?.ApiKey ?? "YourApiKey";
                this.Region = config?.Region ?? "YourServiceRegion";
            }
            catch (Exception)
            {
                //MessageBox.Show("Error reading config file. Ensure it's in the correct format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Application.Exit();
                TerminalMessageBoxNeeded = true;
                TerminalMessageBoxText = "Error reading config file. Ensure it's in the correct format.";
            }
        }
    }

    private void SaveDefaultConfig()
    {
        var defaultConfig = new ConfigManager(); // Uses default ApiKey and Region
        string jsonString = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ConfigFileName, jsonString);
    }
}
