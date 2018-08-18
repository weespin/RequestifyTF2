﻿using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using RequestifyTF2.API;
using RequestifyTF2GUI.Controls;

namespace RequestifyTF2GUI
{
    internal static class AppConfig
    {
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
                    Requestify.Admin = CurrentConfig.Admin;
                }
                else
                {
                    File.WriteAllText(
                        Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json",
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
            {
                MessageBox.Show("Please set the game directory", "Error");
            }

            CurrentConfig.Debug = Requestify.Debug;
            ConsoleTab.instance.debugchk.IsChecked = CurrentConfig.Debug;
            
            if (CurrentConfig.Buttons == null)
            {
                CurrentConfig.Buttons = new Buttons();
                CurrentConfig.Buttons.buttons = new List<BindsViewModel>(10);
                for (int i = 0; i < 10; i++)
                {
                    CurrentConfig.Buttons.buttons.Add(new BindsViewModel()
                    {
                        Id = i,
                        BindType = "LocalMusic",
                        IsSelected = true,
                        Link = string.Empty,
                        NumpadKey = "NUMPAD " + i
                    });
                }
            }

            Requestify.GameDir = CurrentConfig.GameDirectory;
            AppConfig.Save();
        }

        public static void Save()
        {
            Requestify.GameDir = CurrentConfig.GameDirectory;
            var currentconfig = JsonConvert.SerializeObject(CurrentConfig);
            CurrentConfig.Admin = Requestify.Admin;
            File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json", currentconfig);
        }

        internal class ConfigJsonData
        {
       
            [JsonProperty("Admin")] public string Admin { get; set; }
            [JsonProperty("GameDirectory")] public string GameDirectory { get; set; }
            [JsonProperty("CoreLang")] public string CoreLang { get; set; }
            [JsonProperty("DebugMode")] public bool Debug { get; set; }
            [JsonProperty("ButtonBinds")]
            public Buttons Buttons { get; set; }

        }

        internal class Buttons
        {

            public List<BindsViewModel> buttons = new List<BindsViewModel>();
        }
    }
}