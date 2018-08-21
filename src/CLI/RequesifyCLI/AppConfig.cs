// RequesifyCLI
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