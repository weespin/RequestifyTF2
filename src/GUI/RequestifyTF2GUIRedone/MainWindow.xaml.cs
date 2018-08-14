using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CSCore.Codecs.MP3;
using CSCore.SoundOut;
using Microsoft.Win32;
using Ookii.Dialogs;
using RequestifyTF2;
using RequestifyTF2.API;
using RequestifyTF2.Utils;
using RequestifyTF2GUIRedone.Controls;

namespace RequestifyTF2GUIRedone
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance;
        public bool _started;


        private TextWriter _writer;

        public MainWindow()
        {
            InitializeComponent();
            new Thread(StatsMonitor).Start();
            AppConfig.Load();
            SettingsTab.instance.GamePath.Text= AppConfig.CurrentConfig.GameDirectory;
            Main.instance.AdminBox.Text = AppConfig.CurrentConfig.Admin;
            App.LanguageChanged += LanguageChanged;
            instance = this;
         
            CultureInfo currLang = App.Language;

            //Заполняем меню смены языка:
            SettingsTab.instance.menuLanguage.Items.Clear();
            foreach (var lang in App.Languages)
            {
                MenuItem menuLang = new MenuItem
                {
                    Header = lang.DisplayName,
                    Tag = lang,
                    IsChecked = lang.Equals(currLang)
                };
                menuLang.Click += ChangeLanguageClick;
                SettingsTab.instance.menuLanguage.Items.Add(menuLang);
               
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
                SettingsTab.instance.ComboBoxCoreLanguage.Items.Add(menuLang);
            }

            if (AppConfig.CurrentConfig.CoreLang == null)
            {
                AppConfig.CurrentConfig.CoreLang = "en";
                AppConfig.Save();
            }
           currLang = new CultureInfo(AppConfig.CurrentConfig.CoreLang);
            foreach (MenuItem i in SettingsTab.instance.ComboBoxCoreLanguage.Items)
            {
                CultureInfo ci = (CultureInfo)i.Tag;
                i.IsChecked = ci != null && ci.Name==currLang.Name;
            }

            ChangeCoreLang(currLang);
        }
        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;
            foreach (MenuItem i in SettingsTab.instance.menuLanguage.Items)
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
            foreach (MenuItem i in SettingsTab.instance.ComboBoxCoreLanguage.Items)
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
                SettingsTab.instance.LinesParsed.Dispatcher.Invoke(() => { SettingsTab.instance.LinesParsed.Content = Application.Current.FindResource("s_Lines_Parsed").ToString()+Statisctics.LinesParsed.ToString(); });

                SettingsTab.instance.CommandsParsed.Dispatcher.Invoke(() =>
                {
                    SettingsTab.instance.CommandsParsed.Content = Application.Current.FindResource("s_Commands_Parsed").ToString()+Statisctics.CommandsParsed.ToString();
                });
                SettingsTab.instance.GameKills.Dispatcher.Invoke(() => { SettingsTab.instance.GameKills.Content = Application.Current.FindResource("s_Game_Kills").ToString()+Statisctics.GameKills.ToString(); });
                SettingsTab.instance.YourKills.Dispatcher.Invoke(() => { SettingsTab.instance.YourKills.Content = Application.Current.FindResource("s_Your_Kills").ToString()+Statisctics.YourKills.ToString(); });
                SettingsTab.instance.TotalKills.Dispatcher.Invoke(() =>
                {
                    SettingsTab.instance.TotalKills.Content = Application.Current.FindResource("s_Total_Kills").ToString()+Convert.ToString(Statisctics.YourKills + Statisctics.GameKills);
                });
                SettingsTab.instance.YourCritKills.Dispatcher.Invoke(() =>
                {
                    SettingsTab.instance.YourCritKills.Content = Application.Current.FindResource("s_Your_Crits_Kills").ToString() + Statisctics.YourCritsKill.ToString();
                });
                SettingsTab.instance.GameCritKills.Dispatcher.Invoke(() => { SettingsTab.instance.GameCritKills.Content = Application.Current.FindResource("s_Game_Crits_Kills").ToString() + Statisctics.CritsKill.ToString(); });
                SettingsTab.instance.TotalCritKills.Dispatcher.Invoke(() =>
                {
                    SettingsTab.instance.TotalCritKills.Content = Application.Current.FindResource("s_Total_Crits_Kills").ToString() + Convert.ToString(Statisctics.CritsKill + Statisctics.YourCritsKill);
                });
                SettingsTab.instance.YourDeaths.Dispatcher.Invoke(() => { SettingsTab.instance.YourDeaths.Content = Application.Current.FindResource("s_Your_Deaths").ToString() + Statisctics.YourDeaths.ToString(); });
                SettingsTab.instance.GameDeaths.Dispatcher.Invoke(() => { SettingsTab.instance.GameDeaths.Content = Application.Current.FindResource("s_Game_Deaths").ToString() + Statisctics.Deaths.ToString(); });
                SettingsTab.instance.TotalDeaths.Dispatcher.Invoke(() =>
                {
                    SettingsTab.instance.TotalDeaths.Content = Application.Current.FindResource("s_Total_Deaths").ToString() + Convert.ToString(Statisctics.YourDeaths + Statisctics.Deaths);
                });
                SettingsTab.instance.YourSuicides.Dispatcher.Invoke(() => { SettingsTab.instance.YourSuicides.Content = Application.Current.FindResource("s_Your_Suicides").ToString() + Statisctics.YourSuicides.ToString(); });
                SettingsTab.instance.GameSuicides.Dispatcher.Invoke(() => { SettingsTab.instance.GameSuicides.Content = Application.Current.FindResource("s_Game_Suicides").ToString() + Statisctics.Suicides.ToString(); });
                SettingsTab.instance.TotalSuicides.Dispatcher.Invoke(() =>
                {
                    SettingsTab.instance.TotalSuicides.Content = Application.Current.FindResource("s_Total_Suicides").ToString() + Convert.ToString(Statisctics.Suicides + Statisctics.YourSuicides);
                });
                IgnoreListTab.instance.Attempts.Dispatcher.Invoke(() =>
                {
                    IgnoreListTab.instance.Attempts.Content = Application.Current.FindResource("i_Attempts").ToString() + Statisctics.IgnoreListStopped;
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


        public void UndefinedMessage_OnUndefinedMessage(Events.UndefinedMessageArgs e)
        {
            var mes = e.Message.Trim();
            if (mes.StartsWith("NUMPAD"))
            {
                mes = mes.Replace("NUMPAD", "");
                if (mes.Length > 0)
                {
                    Play(Convert.ToInt32(mes));
                }
            }
        }
        private void Play(int id)
        {
            if (AppConfig.CurrentConfig.Buttons.buttons.Length > id)
            {
                if (AppConfig.CurrentConfig.Buttons.buttons[id] != null)
                {
                    if (File.Exists(AppConfig.CurrentConfig.Buttons.buttons[id].Link))
                    {
                        if (Instance.SoundOutExtra.PlaybackState == PlaybackState.Paused ||
                            Instance.SoundOutExtra.PlaybackState == PlaybackState.Playing)
                        {
                            Instance.SoundOutExtra.Stop();
                        }

                        Instance.SoundOutExtra.Initialize(
                            new Mp3MediafoundationDecoder(AppConfig.CurrentConfig.Buttons.buttons[id].Link));
                        Instance.SoundOutExtra.Play();
                    }
                }
            }
        }
        
        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            _writer = new TextBoxStreamWriter(ConsoleTab.instance.Console);
            System.Console.SetOut(_writer);
            var plugins = Instance.Plugins.GetPlugins();
            if (plugins.Count == 0)
            {
                Logger.Write(Logger.Status.Error, Application.Current.FindResource("cs_Cant_Find_Plugins").ToString());
            }

            foreach (var item in plugins)
            {
                PluginsTab.instance.PluginsList.Items.Add(new PluginItem {Plugin = item.plugin, PluginName = item.plugin.Name});
               //
            }

            foreach (var com in Instance.Commands.GetCommands())
            {
                PluginsTab.instance.CommandsBox.Items.Add(new CommandItem {Command = com.ICommand, CommandName = com.Name});
            }
        }

       

      
       

        



        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        public class PluginItem
        {
            public string PluginName { get; set; }
            public IRequestifyPlugin Plugin { get; set; }
            public bool Enabled { get; set; }
        }

        public class CommandItem
        {
            public string CommandName { get; set; }
            public IRequestifyCommand Command { get; set; }
            public bool Enabled { get; set; }
        }

      

       
    }
}