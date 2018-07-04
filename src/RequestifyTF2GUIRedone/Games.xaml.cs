using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;

namespace RequestifyTF2GUIRedone
{
    /// <summary>
    /// Логика взаимодействия для Games.xaml
    /// </summary>
    public partial class Games : Window
    {
        public Games()
        {
            InitializeComponent();
        }

        public class SteamGame
        {
            public int id { get; set; }
            public string photolink { get; set; }
            public string Name { get; set; }
            

            public string path { get; set; }
        }
      public static  List<SteamGame> SteamIdList = new List<SteamGame>();
        static void Refresh()
        {
            var ProgramList = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\", RegistryRights.ReadKey).GetSubKeyNames();
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
                    SteamGame game = new SteamGame
                    {
                        id = Convert.ToInt32(a.Groups[1].Value),
                        path = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                            .OpenSubKey($"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{v}",
                                RegistryRights.ReadKey).GetValue("InstallLocation").ToString()
                    };
                    string json;
                    using (WebClient cl = new WebClient())
                    {
                       
                       json = cl.DownloadString(
                           $"https://store.steampowered.com/api/appdetails?appids={a.Groups[1].Value}");
                    }

                    JObject jObject = JObject.Parse(json);
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

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
           Refresh();
        }

        private void SelectClick(object sender, RoutedEventArgs e)
        {
            if (GamesList.SelectedItem != null)
            {
                var a = (SteamGame) GamesList.SelectedItem;
             
             var path=   RequestifyTF2.Utils.Patcher.ResolveFolder(a.path);
                if (path != "")
                {
                    AppConfig.CurrentConfig.GameDirectory = path;
                    AppConfig.Save();
                  
                    this.Close();
                }
                else
                {
                    MessageBox.Show("This is not a Source Engine game", "Error",
                        MessageBoxButton.OK);
                }
                  
                
            
        }
        }

        private void GamesList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var a = (SteamGame)GamesList.SelectedItem;

            var path = RequestifyTF2.Utils.Patcher.ResolveFolder(a.path);
            if (path != "")
            {
                AppConfig.CurrentConfig.GameDirectory = path;
                AppConfig.Save();
                MainWindow page1 = (MainWindow)this.DataContext;
                page1.GamePath.Content = path;
                this.Close();
            }
            else
            {
                MessageBox.Show("This is not a Source Engine game", "Error",
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
                DragMove();
        }

    }
}
