using System;
using System.IO;
using Newtonsoft.Json;
using RequestifyTF2.API;

namespace RequesifyCLI
{
    internal static class AppConfig
    {
        public static ConfigJsonData CurrentConfig { get; set; } = new ConfigJsonData();

        public static void Load()
        {
            var emptyjson = JsonConvert.SerializeObject(
                new ConfigJsonData {GameDirectory = string.Empty, Admin = "null"});
            if (Directory.Exists(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/config/"))
            {
                if (File.Exists(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/config/config.json"))
                {
                    CurrentConfig = JsonConvert.DeserializeObject<ConfigJsonData>(
                        File.ReadAllText(
                            Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/config/config.json"));
                    Requestify.Admin = CurrentConfig.Admin;
                }
                else
                {
                    File.WriteAllText(
                       Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "config/config.json",
                        emptyjson);

                    Logger.Nlogger.Info("Type dir {directory} to set directory");
                }
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/config/");
                File.WriteAllText(
                    Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/config/config.json",
                    emptyjson);
                Logger.Nlogger.Info("Type dir {directory} to set directory");
            }

            if (CurrentConfig.GameDirectory == string.Empty)
            {
                Logger.Nlogger.Info("Type dir {directory} to set directory");
            }

            Requestify.GameDir = CurrentConfig.GameDirectory;
        }

        public static void Save()
        {
            Requestify.GameDir = CurrentConfig.GameDirectory;
            var currentconfig = JsonConvert.SerializeObject(CurrentConfig);
            File.WriteAllText(
                Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/config/config.json",
                currentconfig);
        }
    }
}