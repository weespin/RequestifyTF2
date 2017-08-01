using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using RequestifyTF2.Api;
using RequestifyTF2Forms.JsonDataClasses;

namespace RequestifyTF2Forms.Config
{
    internal static class AppConfig
    {
        public static ConfigJsonData Crntcfg = new ConfigJsonData();
        //todo:  Write not text, write fucking class into json.
        public static void Load()
        {
            if (Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/config/"))
            {
                if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json"))
                {
                    Crntcfg = JsonConvert.DeserializeObject<ConfigJsonData>(File.ReadAllText(
                        Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json"));
                }
                else
                {
                    File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "config/config.json",
                        "{\r\n  \"GameDir\": \"\",\r\n  \"OnlyAdmin\": false\r\n}");
                    MessageBox.Show("Please select game directory in 'Settings' menu", "Warning", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "/config/");
                File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json",
                    "{\r\n  \"GameDir\": \"\",\r\n  \"OnlyAdmin\": false\r\n}");
                MessageBox.Show("Please select game directory in 'Settings' menu", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            if (Crntcfg.GameDir == "")
                MessageBox.Show("Please select game directory in 'Settings' menu", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            Instances.Config.GameDir = Crntcfg.GameDir;
        }

        public static void Save()
        {
            Instances.Config.GameDir = Crntcfg.GameDir;
            var currentconfig = JsonConvert.SerializeObject(Crntcfg);
            File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json", currentconfig);
        }
    }
}