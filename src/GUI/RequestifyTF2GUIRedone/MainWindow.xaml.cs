using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Ookii.Dialogs;
using RequestifyTF2;
using RequestifyTF2.API;
using RequestifyTF2.Utils;

namespace RequestifyTF2GUIRedone
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _started;


        private TextWriter _writer;

        public MainWindow()
        {
            InitializeComponent();
            new Thread(StatsMonitor).Start();
            AppConfig.Load();
            GamePath.Content = AppConfig.CurrentConfig.GameDirectory;
            AdminBox.Text = AppConfig.CurrentConfig.Admin;
            App.LanguageChanged += LanguageChanged;
           
         
            CultureInfo currLang = App.Language;
           
            //Заполняем меню смены языка:
            menuLanguage.Items.Clear();
            foreach (var lang in App.Languages)
            {
                MenuItem menuLang = new MenuItem
                {
                    Header = lang.DisplayName,
                    Tag = lang,
                    IsChecked = lang.Equals(currLang)
                };
                menuLang.Click += ChangeLanguageClick;
                menuLanguage.Items.Add(menuLang);
               
            }
            foreach (var lang in App.Languages)
            {
                MenuItem menuLang = new MenuItem
                {
                    Header = lang.DisplayName,
                    Tag = lang,
                    IsChecked = lang.Equals(Instance.GetCulture)
                };
                menuLang.Click += ChangeCoreLanguageClick;
                menuCoreLanguage.Items.Add(menuLang);
            }

            if (AppConfig.CurrentConfig.CoreLang == null)
            {
                AppConfig.CurrentConfig.CoreLang = "en";
                AppConfig.Save();
            }
           currLang = new CultureInfo(AppConfig.CurrentConfig.CoreLang);
            foreach (MenuItem i in menuCoreLanguage.Items)
            {
                CultureInfo ci = (CultureInfo)i.Tag;
                i.IsChecked = ci != null && ci.Name==currLang.Name;
            }

            ChangeCoreLang(currLang);
        }
        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;
            foreach (MenuItem i in menuLanguage.Items)
            {
                var ci = (CultureInfo) i.Tag;
                if (ci != null && ci.Equals(currLang))
                {
                    i.IsChecked = true;
                }
                else
                {
                    i.IsChecked =false;
                }
            }
        }

        private void ChangeLanguageClick(Object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            if (mi!=null)
            {
                var lang = (CultureInfo) mi.Tag;
                if (lang != null)
                {
                  
                        App.Language = lang;
                    
                }
            }

        }
      
        private void ChangeCoreLanguageClick(Object sender, EventArgs e)
        {
            var mi = (MenuItem) sender;
            var lang = (CultureInfo) mi?.Tag;
            if (lang == null)
            {
                return;
            }

            ChangeCoreLang(lang);
            foreach (MenuItem i in menuCoreLanguage.Items)
            {
                CultureInfo ci = (CultureInfo)i.Tag;
                if (ci != null)
                {
                    if (ci.Name.Equals(lang.Name))
                    {
                        i.IsChecked = true;
                    }
                    else
                    {
                        i.IsChecked = false;
                    }
                }
            }
            AppConfig.CurrentConfig.CoreLang = lang.TextInfo.CultureName;
            AppConfig.Save();
        }

        private void ChangeCoreLang(CultureInfo lang)
        {
            if (lang.Name.Contains("bg"))
            {
                Instance.Language = Instance.ELanguage.BG;
            }
            else if (lang.Name.Contains( "cs"))
            {
                Instance.Language = Instance.ELanguage.CS;
            }
            else if (lang.Name.Contains( "da"))
            {
                Instance.Language = Instance.ELanguage.DA;
            }
            else if (lang.Name.Contains( "de"))
            {
                Instance.Language = Instance.ELanguage.DE;
            }
            else if (lang.Name.Contains( "el"))
            {
                Instance.Language = Instance.ELanguage.EL;
            }
            else if (lang.Name.Contains( "es"))
            {
                Instance.Language = Instance.ELanguage.ES;
            }
            else if (lang.Name.Contains( "fi"))
            {
                Instance.Language = Instance.ELanguage.FI;
            }
            else if (lang.Name.Contains( "fr"))
            {
                Instance.Language = Instance.ELanguage.FR;
            }
            else if (lang.Name.Contains( "hu"))
            {
                Instance.Language = Instance.ELanguage.HU;
            }
            else if (lang.Name.Contains( "it"))
            {
                Instance.Language = Instance.ELanguage.IT;
            }
            else if (lang.Name.Contains( "ja"))
            {
                Instance.Language = Instance.ELanguage.JA;
            }
            else if (lang.Name.Contains( "ko"))
            {
                Instance.Language = Instance.ELanguage.KO;
            }
            else if (lang.Name.Contains( "nl"))
            {
                Instance.Language = Instance.ELanguage.NL;
            }
            else if (lang.Name.Contains( "nn"))
            {
                Instance.Language = Instance.ELanguage.NN;
            }
            else if (lang.Name.Contains( "pt"))
            {
                if (lang.Name.Contains("BR"))
                {
                    Instance.Language = Instance.ELanguage.BR;
                }
                else
                {
                    Instance.Language = Instance.ELanguage.PT;
                }
            }
            else if (lang.Name.Contains("en"))
            {
                Instance.Language = Instance.ELanguage.EN;
            }
            else if (lang.Name.Contains("ro"))
            {
                Instance.Language = Instance.ELanguage.RO;
            }
            else if (lang.Name.Contains("ru"))
            {
                Instance.Language = Instance.ELanguage.RU;
            }
            else if (lang.Name.Contains("sv"))
            {
                Instance.Language = Instance.ELanguage.SV;
            }
            else if (lang.Name.Contains("th"))
            {
                Instance.Language = Instance.ELanguage.TH;
            }
            else if (lang.Name.Contains("tr"))
            {
                Instance.Language = Instance.ELanguage.TR;
            }
            else if (lang.Name.Contains("uk"))
            {
                Instance.Language = Instance.ELanguage.UK;
            }
            else if (lang.Name.Contains("zh"))
            {
                if (lang.Name.Contains("CN"))
                {
                    Instance.Language = Instance.ELanguage.SZN;
                }
                else
                {
                    Instance.Language = Instance.ELanguage.TZN;
                }
            }
            else
            {
                Instance.Language = Instance.ELanguage.EN;
            }
        }
        private void StatsMonitor()
        {
            while (true)
            {
                LinesParsed.Dispatcher.Invoke(() => { LinesParsed.Content = Application.Current.FindResource("s_Lines_Parsed").ToString()+Statisctics.LinesParsed.ToString(); });

                CommandsParsed.Dispatcher.Invoke(() =>
                {
                    CommandsParsed.Content = Application.Current.FindResource("s_Commands_Parsed").ToString()+Statisctics.CommandsParsed.ToString();
                });
                GameKills.Dispatcher.Invoke(() => { GameKills.Content = Application.Current.FindResource("s_Game_Kills").ToString()+Statisctics.GameKills.ToString(); });
                YourKills.Dispatcher.Invoke(() => { YourKills.Content = Application.Current.FindResource("s_Your_Kills").ToString()+Statisctics.YourKills.ToString(); });
                TotalKills.Dispatcher.Invoke(() =>
                {
                    TotalKills.Content = Application.Current.FindResource("s_Total_Kills").ToString()+Convert.ToString(Statisctics.YourKills + Statisctics.GameKills);
                });
                YourCritKills.Dispatcher.Invoke(() =>
                {
                    YourCritKills.Content = Application.Current.FindResource("s_Your_Crits_Kills").ToString() + Statisctics.YourCritsKill.ToString();
                });
                GameCritKills.Dispatcher.Invoke(() => { GameCritKills.Content = Application.Current.FindResource("s_Game_Crits_Kills").ToString() + Statisctics.CritsKill.ToString(); });
                TotalCritKills.Dispatcher.Invoke(() =>
                {
                    TotalCritKills.Content = Application.Current.FindResource("s_Total_Crits_Kills").ToString() + Convert.ToString(Statisctics.CritsKill + Statisctics.YourCritsKill);
                });
                YourDeaths.Dispatcher.Invoke(() => { YourDeaths.Content = Application.Current.FindResource("s_Your_Deaths").ToString() + Statisctics.YourDeaths.ToString(); });
                GameDeaths.Dispatcher.Invoke(() => { GameDeaths.Content = Application.Current.FindResource("s_Game_Deaths").ToString() + Statisctics.Deaths.ToString(); });
                TotalDeaths.Dispatcher.Invoke(() =>
                {
                    TotalDeaths.Content = Application.Current.FindResource("s_Total_Deaths").ToString() + Convert.ToString(Statisctics.YourDeaths + Statisctics.Deaths);
                });
                YourSuicides.Dispatcher.Invoke(() => { YourSuicides.Content = Application.Current.FindResource("s_Your_Suicides").ToString() + Statisctics.YourSuicides.ToString(); });
                GameSuicides.Dispatcher.Invoke(() => { GameSuicides.Content = Application.Current.FindResource("s_Game_Suicides").ToString() + Statisctics.Suicides.ToString(); });
                TotalSuicides.Dispatcher.Invoke(() =>
                {
                    TotalSuicides.Content = Application.Current.FindResource("s_Total_Suicides").ToString() + Convert.ToString(Statisctics.Suicides + Statisctics.YourSuicides);
                });
                Attempts.Dispatcher.Invoke(() =>
                {
                   Attempts.Content = Application.Current.FindResource("i_Attempts").ToString() + Statisctics.IgnoreListStopped;
                });
                Thread.Sleep(250);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (Instance.Config.GameDir == string.Empty)
            {
                MessageBox.Show(
                    Application.Current.FindResource("cs_Set_Game_Dir").ToString(),
                        Application.Current.FindResource("cs_Error").ToString());

                return;
            }
            _started = Runner.Start();
            if (_started)
            {
                StartButton.Content = Application.Current.FindResource("cs_Stop").ToString();
                StatusLabel.Content = Application.Current.FindResource("cs_Status_Working").ToString();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var s = new VistaFolderBrowserDialog())
            {
                s.UseDescriptionForTitle = true;
                s.Description = Application.Current.FindResource("cs_Select_Game_Path").ToString();

                if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (s.SelectedPath == string.Empty)
                    {
                        return;
                    }

                    var path = Patcher.ResolveFolder(s.SelectedPath);
                    if (path != "")
                    {
                        GamePath.Content = Application.Current.FindResource("cs_Current_Game_Path").ToString() + path;
                        AppConfig.CurrentConfig.GameDirectory = path;
                        AppConfig.Save();

                        Close();
                    }
                    else
                    {
                        MessageBox.Show(Application.Current.FindResource("cs_Not_Source_Engine_Game").ToString(), Application.Current.FindResource("cs_Error").ToString(),
                            MessageBoxButton.OK);
                    }      
                }
            }
        }
        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            _writer = new TextBoxStreamWriter(Console, ConsoleLabel2);
            System.Console.SetOut(_writer);
            var plugins = Instance.Plugins.GetPlugins();
            if (plugins.Count == 0)
            {
                Logger.Write(Logger.Status.Error, Application.Current.FindResource("cs_Cant_Find_Plugins").ToString());
            }

            foreach (var item in plugins)
            {
                PluginsList.Items.Add(new PluginItem {Plugin = item.plugin, PluginName = item.plugin.Name});
            }

            foreach (var com in Instance.Commands.GetCommands())
            {
                CommandsBox.Items.Add(new CommandItem {Command = com.ICommand, CommandName = com.Name});
            }
        }

        private void PluginsList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var a = PluginsList.SelectedItem as PluginItem;
            var index = PluginsList.SelectedIndex;
            if (a != null)
            {
                PluginsList.Items.RemoveAt(index);
                if (a.Color == null)
                {
                    a.Color = (Brush) new BrushConverter().ConvertFrom("#87b91d47");
                    Instance.Plugins.DisablePlugin(Instance.Plugins.GetPlugin(a.PluginName));
                }
                else
                {
                    Instance.Plugins.EnablePlugin(Instance.Plugins.GetPlugin(a.PluginName));
                    a.Color = null;
                }

                PluginsList.Items.Insert(index, a);
            }
        }

        private void CommandsBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var a = CommandsBox.SelectedItem as CommandItem;
            var index = CommandsBox.SelectedIndex;
            if (a != null)
            {
                CommandsBox.Items.RemoveAt(index);
                if (a.Color == null)
                {
                    a.Color = (Brush) new BrushConverter().ConvertFrom("#87b91d47");
                    Instance.Commands.DisableCommand(Instance.Commands.GetCommand(a.Command.Name));
                }
                else
                {
                    Instance.Commands.EnableCommand(Instance.Commands.GetCommand(a.Command.Name));
                    a.Color = null;
                }

                CommandsBox.Items.Insert(index, a);
            }
        }

        private void IgnoreListButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            if (!IgnoreList.Items.Contains(IgnoreBox.Text))
            {
                IgnoreList.Items.Add(IgnoreBox.Text);
                if (!Instance.Config.Ignored.Contains(IgnoreBox.Text))
                {
                    Instance.Config.Ignored.Add(IgnoreBox.Text);
                }
            }
        }

        private void IgnoreListButtonRemove_OnClick(object sender, RoutedEventArgs e)
        {
            if (IgnoreList.Items.Contains(IgnoreBox.Text))
            {
                if (Instance.Config.Ignored.Contains(IgnoreBox.Text))
                {
                    Instance.Config.Ignored.Remove(IgnoreBox.Text);
                }

                IgnoreList.Items.Remove(IgnoreBox.Text);
                return;
            }

            if (IgnoreList.SelectedItem != null)
            {
                if (Instance.Config.Ignored.Contains(IgnoreList.SelectedItem))
                {
                    Instance.Config.Ignored.Remove(IgnoreList.SelectedItem.ToString());
                }

                IgnoreList.Items.Remove(IgnoreList.SelectedItem);
            }
        }

        private void RemoveCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            Instance.Config.IgnoredReversed = true;
        }

        private void RemoveCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            Instance.Config.IgnoredReversed = false;
        }

        private void GameBrowser_click(object sender, RoutedEventArgs e)
        {
            var a = new Games {DataContext = this};
            a.Show();
        }


        private void AdminBoxFocusLost(object sender, RoutedEventArgs e)
        {
            AppConfig.CurrentConfig.Admin = AdminBox.Text;
            AppConfig.Save();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        public class PluginItem
        {
            public string PluginName { get; set; }
            public IRequestifyPlugin Plugin { get; set; }
            public Brush Color { get; set; }
        }

        public class CommandItem
        {
            public string CommandName { get; set; }
            public IRequestifyCommand Command { get; set; }
            public Brush Color { get; set; }
        }

        private void MutedCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            Instance.IsMuted = true;
        }

        private void MutedCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            Instance.IsMuted = false;
        }

    }
}