using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using RequestifyTF2.Api;

internal static class AppConfig
{

    internal class ConfigJsonData
    {
        [JsonProperty("Admin")] public string Admin { get; set; }

        [JsonProperty("GameDirectory")] public string GameDirectory { get; set; }


    }

    public static ConfigJsonData CurrentConfig = new ConfigJsonData();

    public static void Load()
    {
        var emptyjson = JsonConvert.SerializeObject(
            new ConfigJsonData {GameDirectory = string.Empty, Admin = string.Empty});
        if (Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/config/"))
        {
            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json"))
            {
                CurrentConfig = JsonConvert.DeserializeObject<ConfigJsonData>(
                    File.ReadAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json"));
                Instance.Config.Admin = CurrentConfig.Admin;
            }
            else
            {
                File.WriteAllText(
                    Path.GetDirectoryName(Application.ExecutablePath) + "config/config.json",
                    emptyjson);

                MessageBox.Show("Please set the game directory", "Error");
            }
        }
        else
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "/config/");
            File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json", emptyjson);
            MessageBox.Show("Please set the game directory", "Error");
        }

        if (CurrentConfig.GameDirectory == string.Empty)
            MessageBox.Show("Please set the game directory", "Error");
        Instance.Config.GameDir = CurrentConfig.GameDirectory;
    }

    public static void Save()
    {
        Instance.Config.GameDir = CurrentConfig.GameDirectory;
        var currentconfig = JsonConvert.SerializeObject(CurrentConfig);
        CurrentConfig.Admin = Instance.Config.Admin;
        File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json", currentconfig);
    }
}