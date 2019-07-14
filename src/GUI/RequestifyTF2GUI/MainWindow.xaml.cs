using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CSCore;
using CSCore.Codecs.AAC;
using CSCore.Codecs.MP3;
using CSCore.SoundOut;
using RequestifyTF2.API;
using RequestifyTF2.Utils;
using RequestifyTF2GUI.Controls;
using AudioEncoding = YoutubeExplode.Models.MediaStreams.AudioEncoding;

namespace RequestifyTF2GUI
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
            SettingsTab.instance.GamePath.Text= AppConfig.CurrentConfig.GameDirectory;
            SettingsTab.instance.AdminBox.Text = AppConfig.CurrentConfig.Admin;
            SettingsTab.instance.txtNum2.Text = AppConfig.CurrentConfig.AntiSpamThredshold.ToString();
            SettingsTab.instance.txtNum.Text = AppConfig.CurrentConfig.MaximumBackgroundInMin.ToString();
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
               StatisticsTab.instance.LinesParsed.Dispatcher.Invoke(() => {StatisticsTab.instance.LinesParsed.Text = Application.Current.FindResource("s_Lines_Parsed").ToString()+Statisctics.LinesParsed.ToString(); });

               StatisticsTab.instance.CommandsParsed.Dispatcher.Invoke(() =>
                {
                   StatisticsTab.instance.CommandsParsed.Text = Application.Current.FindResource("s_Commands_Parsed").ToString()+Statisctics.CommandsParsed.ToString();
                });
               StatisticsTab.instance.GameKills.Dispatcher.Invoke(() => {StatisticsTab.instance.GameKills.Text = Application.Current.FindResource("s_Game_Kills").ToString()+Statisctics.GameKills.ToString(); });
               StatisticsTab.instance.YourKills.Dispatcher.Invoke(() => {StatisticsTab.instance.YourKills.Text = Application.Current.FindResource("s_Your_Kills").ToString()+Statisctics.YourKills.ToString(); });
               StatisticsTab.instance.TotalKills.Dispatcher.Invoke(() =>
                {
                   StatisticsTab.instance.TotalKills.Text = Application.Current.FindResource("s_Total_Kills").ToString()+Convert.ToString(Statisctics.YourKills + Statisctics.GameKills);
                });
               StatisticsTab.instance.YourCritKills.Dispatcher.Invoke(() =>
                {
                   StatisticsTab.instance.YourCritKills.Text = Application.Current.FindResource("s_Your_Crits_Kills").ToString() + Statisctics.YourCritsKill.ToString();
                });
               StatisticsTab.instance.GameCritKills.Dispatcher.Invoke(() => {StatisticsTab.instance.GameCritKills.Text = Application.Current.FindResource("s_Game_Crits_Kills").ToString() + Statisctics.CritsKill.ToString(); });
               StatisticsTab.instance.TotalCritKills.Dispatcher.Invoke(() =>
                {
                   StatisticsTab.instance.TotalCritKills.Text = Application.Current.FindResource("s_Total_Crits_Kills").ToString() + Convert.ToString(Statisctics.CritsKill + Statisctics.YourCritsKill);
                });
               StatisticsTab.instance.YourDeaths.Dispatcher.Invoke(() => {StatisticsTab.instance.YourDeaths.Text = Application.Current.FindResource("s_Your_Deaths").ToString() + Statisctics.YourDeaths.ToString(); });
               StatisticsTab.instance.GameDeaths.Dispatcher.Invoke(() => {StatisticsTab.instance.GameDeaths.Text = Application.Current.FindResource("s_Game_Deaths").ToString() + Statisctics.Deaths.ToString(); });
               StatisticsTab.instance.TotalDeaths.Dispatcher.Invoke(() =>
                {
                   StatisticsTab.instance.TotalDeaths.Text = Application.Current.FindResource("s_Total_Deaths").ToString() + Convert.ToString(Statisctics.YourDeaths + Statisctics.Deaths);
                });
               StatisticsTab.instance.YourSuicides.Dispatcher.Invoke(() => {StatisticsTab.instance.YourSuicides.Text = Application.Current.FindResource("s_Your_Suicides").ToString() + Statisctics.YourSuicides.ToString(); });
               StatisticsTab.instance.GameSuicides.Dispatcher.Invoke(() => {StatisticsTab.instance.GameSuicides.Text = Application.Current.FindResource("s_Game_Suicides").ToString() + Statisctics.Suicides.ToString(); });
               StatisticsTab.instance.TotalSuicides.Dispatcher.Invoke(() =>
                {
                   StatisticsTab.instance.TotalSuicides.Text = Application.Current.FindResource("s_Total_Suicides").ToString() + Convert.ToString(Statisctics.Suicides + Statisctics.YourSuicides);
                });
               SettingsTab.instance.Attempts.Dispatcher.Invoke(() =>
                {
                   SettingsTab.instance.Attempts.Content = Application.Current.FindResource("i_Attempts").ToString() + " " + Statisctics.IgnoreListStopped;
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
                    int s;
                    if (int.TryParse(mes, out s))
                    {
                        Play(s);
                    }
                }
            }
        }
        private void Play(int id)
        {
            if (AppConfig.CurrentConfig.Buttons.buttons.Count> id)
            {
                if (AppConfig.CurrentConfig.Buttons.buttons[id] != null)
                {
                    var b = AppConfig.CurrentConfig.Buttons.buttons[id];
                    if (b.BindType == "YoutubeMusic"&&b.Link!=null)
                    {
                        var cl = new YoutubeExplode.YoutubeClient();
                        var vid = YoutubeExplode.YoutubeClient.ParseVideoId(b.Link);
                        var streamInfoSet = cl.GetVideoMediaStreamInfosAsync(vid);
                        if (streamInfoSet == null)
                        {
                            return;
                        }
                        var streamInfo =
                            streamInfoSet.Result.Audio.FirstOrDefault(n => n.AudioEncoding == AudioEncoding.Aac);
                        if (streamInfo == null)
                        {
                            return;
                        }
                        if (Instance.SoundOutExtra.PlaybackState == PlaybackState.Paused ||
                            Instance.SoundOutExtra.PlaybackState == PlaybackState.Playing)
                        {
                            Instance.SoundOutExtra.Stop();
                        }

                        Instance.SoundOutExtra.Initialize(
                            new AacDecoder(streamInfo.Url).ToMono());
                        Instance.SoundOutExtra.Play();

                    }

                    if (b.BindType == "LocalMusic"&&b.Link!=null)
                    {
                        if (File.Exists(AppConfig.CurrentConfig.Buttons.buttons[id].Link))
                        {
                            if (Instance.SoundOutExtra.PlaybackState == PlaybackState.Paused ||
                                Instance.SoundOutExtra.PlaybackState == PlaybackState.Playing)
                            {
                                Instance.SoundOutExtra.Stop();
                            }

                            Instance.SoundOutExtra.Initialize(
                                new Mp3MediafoundationDecoder(AppConfig.CurrentConfig.Buttons.buttons[id].Link).ToMono());
                            Instance.SoundOutExtra.Play();
                        }
                    }

                    if (b.BindType == "Stop")
                    {
                        if (Instance.SoundOutExtra.PlaybackState == PlaybackState.Paused ||
                            Instance.SoundOutExtra.PlaybackState == PlaybackState.Playing)
                        {
                            Instance.SoundOutExtra.Stop();
                        }
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
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}