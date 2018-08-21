// RequestifyTF2GUIOld(unsupported)
// Copyright (C) 2018  Villiam Nmerukini
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using RequestifyTF2.API;
using RequestifyTF2Forms.JsonDataClasses;

namespace RequestifyTF2Forms.Config
{
    internal static class AppConfig
    {
        public static ConfigJsonData CurrentConfig { get; set; } = new ConfigJsonData();

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
                        Path.GetDirectoryName(Application.ExecutablePath) + "config/config.json",
                        emptyjson);

                    new RequestifyTF2GUI.MessageBox.MessageBox().Show("Please set the game directory", "Error",
                        RequestifyTF2GUI.MessageBox.MessageBox.Sounds.Exclamation);
                }
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "/config/");
                File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json", emptyjson);
                new RequestifyTF2GUI.MessageBox.MessageBox().Show("Please set the game directory", "Error",
                    RequestifyTF2GUI.MessageBox.MessageBox.Sounds.Exclamation);
            }

            if (CurrentConfig.GameDirectory == string.Empty)
            {
                new RequestifyTF2GUI.MessageBox.MessageBox().Show("Please set the game directory", "Error",
                    RequestifyTF2GUI.MessageBox.MessageBox.Sounds.Exclamation);
            }

            Requestify.GameDir = CurrentConfig.GameDirectory;
        }

        public static void Save()
        {
            Requestify.GameDir = CurrentConfig.GameDirectory;
            var currentconfig = JsonConvert.SerializeObject(CurrentConfig);
            CurrentConfig.Admin = Requestify.Admin;
            File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json", currentconfig);
        }
    }
}