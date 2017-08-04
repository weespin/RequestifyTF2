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
                 
                        new RequestifyTF2GUI.MessageBox.MessageBox().Show("Please set the game directory", "Error", RequestifyTF2GUI.MessageBox.MessageBox.Sounds.Exclamation);
                  
                }
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "/config/");
                File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json",
                    emptyjson);
                new RequestifyTF2GUI.MessageBox.MessageBox().Show("Please set the game directory", "Error", RequestifyTF2GUI.MessageBox.MessageBox.Sounds.Exclamation);
            }
            if (CurrentConfig.GameDirectory == "")
                new RequestifyTF2GUI.MessageBox.MessageBox().Show("Please set the game directory", "Error", RequestifyTF2GUI.MessageBox.MessageBox.Sounds.Exclamation);
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