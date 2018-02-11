namespace RequesifyCLI
{
    using System;
    using System.IO;

    using Newtonsoft.Json;

    using RequestifyTF2.Api;

    internal static class AppConfig
    {
        public static ConfigJsonData CurrentConfig = new ConfigJsonData();

        public static void Load()
        {
            var emptyjson = JsonConvert.SerializeObject(
                new ConfigJsonData { GameDirectory = string.Empty, OnlyWithCode = false, Admin = "null" });
            if (Directory.Exists(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/config/"))
            {
                if (File.Exists(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/config/config.json"))
                {
                    CurrentConfig = JsonConvert.DeserializeObject<ConfigJsonData>(
                        File.ReadAllText(
                            Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/config/config.json"));
                    Instance.Config.Admin = CurrentConfig.Admin;
                }
                else
                {
                    File.WriteAllText(
                        Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "config/config.json",
                        emptyjson);

                    Logger.Write(Logger.Status.Info, "Type dir {directory} to set directory");
                }
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/config/");
                File.WriteAllText(
                    Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/config/config.json",
                    emptyjson);
                Logger.Write(Logger.Status.Info, "Type dir {directory} to set directory");
            }

            if (CurrentConfig.GameDirectory == string.Empty)
                Logger.Write(Logger.Status.Info, "Type dir {directory} to set directory");
            Instance.Config.GameDir = CurrentConfig.GameDirectory;
        }

        public static void Save()
        {
            Instance.Config.GameDir = CurrentConfig.GameDirectory;
            var currentconfig = JsonConvert.SerializeObject(CurrentConfig);
            File.WriteAllText(
                Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/config/config.json",
                currentconfig);
        }
    }
}