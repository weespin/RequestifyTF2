using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using RequestifyTF2.Utils;
using RequestifyTF2GUIRedone.Controls;

namespace RequestifyTF2GUIRedone
{
    /// <summary>
    ///     Логика взаимодействия для Games.xaml
    /// </summary>
    public partial class Games : Window
    {
        public static List<SteamGame> SteamIdList { get; set; } = new List<SteamGame>();

        public Games()
        {
            InitializeComponent();
        }

        private static void Refresh()
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
                    if (SteamIdList.Any(n => n.id == Convert.ToInt32(a.Groups[1].Value)))
                    {
                        continue;
                    }

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

                    SteamIdList.Add(game);
                }
            }
        }

        private void Games_OnLoaded(object sender, RoutedEventArgs e)
        {
            Refresh();
            foreach (var games in SteamIdList)
            {
                if (games.Name != null)
                {
                   GamesList.Items.Add(games);
                }
            }
        }

        private void GamesList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var a = (SteamGame) GamesList.SelectedItem;

            var path = Patcher.ResolveFolder(a.path);
            if (path != "")
            {
                AppConfig.CurrentConfig.GameDirectory = path;
                AppConfig.Save();
                var page1 = (MainWindow) DataContext;
                SettingsTab.instance.GamePath.Content = path;
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