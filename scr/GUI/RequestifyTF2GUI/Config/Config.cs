using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using RequestifyTF2.Api;
using RequestifyTF2Forms.JsonDataClasses;

namespace RequestifyTF2Forms.Config
{
    internal static class AppConfig
    {
        public static ConfigJsonData CurrentConfig = new ConfigJsonData();

        
        public static void Load()
        {
            var emptyjson = JsonConvert.SerializeObject(new ConfigJsonData() { GameDirectory = "", OnlyWithCode = false });
            if (Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/config/"))
            {
              
                if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json"))
                {
                    CurrentConfig = JsonConvert.DeserializeObject<ConfigJsonData>(File.ReadAllText(
                        Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json"));
                }
                else
                {
                    File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "config/config.json",
                        emptyjson);
                    System.Windows.Forms.MessageBox.Show("Please select game directory in 'Settings' menu", "Warning", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "/config/");
                File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json",
                    emptyjson);
                System.Windows.Forms.MessageBox.Show("Please select game directory in 'Settings' menu", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            if (CurrentConfig.GameDirectory == "")
                System.Windows.Forms.MessageBox.Show("Please select game directory in 'Settings' menu", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            Instance.Config.GameDir = CurrentConfig.GameDirectory;
        }

        public static void Save()
        {
            Instance.Config.GameDir = CurrentConfig.GameDirectory;
            var currentconfig = JsonConvert.SerializeObject(CurrentConfig);
            File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json", currentconfig);
        }
    }
}