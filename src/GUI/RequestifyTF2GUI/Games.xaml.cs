// RequestifyTF2GUI
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
using System.Net;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using RequestifyTF2.Utils;
using RequestifyTF2GUI.Controls;

namespace RequestifyTF2GUI
{
    /// <summary>
    ///     Логика взаимодействия для Games.xaml
    /// </summary>
    public partial class Games : Window
    {
     

        public Games()
        {
            InitializeComponent();
        }

        public Task Refresh()
        {
           
            var ProgramList = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                .OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\", RegistryRights.ReadKey)
                .GetSubKeyNames();
            var Regex = new Regex(@"Steam App (\d+)");
            foreach (var v in ProgramList)
            {
                var a = Regex.Match(v);
                if (a.Success)
                {


                    var game = new SteamGame
                    {
                        id = Convert.ToInt32(a.Groups[1].Value),
                        path = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                            .OpenSubKey($"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{v}",
                                RegistryRights.ReadKey).GetValue("InstallLocation").ToString()
                    };
                    string json;
                    using (var cl = new WebClient())
                    {
                        json = cl.DownloadString(
                            $"https://store.steampowered.com/api/appdetails?appids={a.Groups[1].Value}");
                    }

                    var jObject = JObject.Parse(json);
                    var root = jObject[a.Groups[1].ToString()].Value<JObject>().ToObject<Root>();
                    if (root.success)
                    {
                        Console.WriteLine(root.data.name);
                        game.Name = root.data.name;
                        game.photolink = root.data.header_image;
                    }

                    if (!GamesList.Items.Contains(game))
                    {
                        dispatcher.Invoke(() => GamesList.Items.Add(game));
                   
                    }
                }
            }

        
            return Task.CompletedTask;
        }

        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        private void Games_OnLoaded(object sender, RoutedEventArgs e)
        {
            Task.Run(Refresh);
            
        }

        private void GamesList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var a = (SteamGame) GamesList.SelectedItem;

            var path = Patcher.ResolveFolder(a.path);
            if (path != "")
            {
                AppConfig.CurrentConfig.GameDirectory = path;
                AppConfig.Save();
                SettingsTab.instance.GamePath.Text = path;
                Close();
            }
            else
            {
                MessageBox.Show(Application.Current.Resources["cs_Not_Source_Engine_Game"].ToString(), Application.Current.Resources["cs_Error"].ToString(),
                    MessageBoxButton.OK);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        public class SteamGame
        {
            public int id { get; set; }
            public string photolink { get; set; }
            public string Name { get; set; }


            public string path { get; set; }
        }

        public class Root
        {
            public bool success { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public string name { get; set; }
            public string header_image { get; set; }
        }
    }
}