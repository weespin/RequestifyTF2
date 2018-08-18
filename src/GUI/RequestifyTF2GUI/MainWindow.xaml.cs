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
using RequestifyTF2.API.Events;
using RequestifyTF2.Audio;
using RequestifyTF2.Managers;
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
            Logger.Nlogger.Info("RequestifyTF2GUI Started");
            InitializeComponent();
            new Thread(StatsMonitor).Start();
            SettingsTab.instance.GamePath.Text= AppConfig.CurrentConfig.GameDirectory;
            SettingsTab.instance.AdminBox.Text = AppConfig.CurrentConfig.Admin;
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
                    IsChecked = lang.Equals(Requestify.GetCulture)
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
                Requestify.Language = Requestify.ELanguage.BG;
            }
            else if (lang.Name.Contains( "cs"))
            {
                Requestify.Language = Requestify.ELanguage.CS;
            }
            else if (lang.Name.Contains( "da"))
            {
                Requestify.Language = Requestify.ELanguage.DA;
            }
            else if (lang.Name.Contains( "de"))
            {
                Requestify.Language = Requestify.ELanguage.DE;
            }
            else if (lang.Name.Contains( "el"))
            {
                Requestify.Language = Requestify.ELanguage.EL;
            }
            else if (lang.Name.Contains( "es"))
            {
                Requestify.Language = Requestify.ELanguage.ES;
            }
            else if (lang.Name.Contains( "fi"))
            {
                Requestify.Language = Requestify.ELanguage.FI;
            }
            else if (lang.Name.Contains( "fr"))
            {
                Requestify.Language = Requestify.ELanguage.FR;
            }
            else if (lang.Name.Contains( "hu"))
            {
                Requestify.Language = Requestify.ELanguage.HU;
            }
            else if (lang.Name.Contains( "it"))
            {
                Requestify.Language = Requestify.ELanguage.IT;
            }
            else if (lang.Name.Contains( "ja"))
            {
                Requestify.Language = Requestify.ELanguage.JA;
            }
            else if (lang.Name.Contains( "ko"))
            {
                Requestify.Language = Requestify.ELanguage.KO;
            }
            else if (lang.Name.Contains( "nl"))
            {
                Requestify.Language = Requestify.ELanguage.NL;
            }
            else if (lang.Name.Contains( "nn"))
            {
                Requestify.Language = Requestify.ELanguage.NN;
            }
            else if (lang.Name.Contains( "pt"))
            {
                if (lang.Name.Contains("BR"))
                {
                    Requestify.Language = Requestify.ELanguage.BR;
                }
                else
                {
                    Requestify.Language = Requestify.ELanguage.PT;
                }
            }
            else if (lang.Name.Contains("en"))
            {
                Requestify.Language = Requestify.ELanguage.EN;
            }
            else if (lang.Name.Contains("ro"))
            {
                Requestify.Language = Requestify.ELanguage.RO;
            }
            else if (lang.Name.Contains("ru"))
            {
                Requestify.Language = Requestify.ELanguage.RU;
            }
            else if (lang.Name.Contains("sv"))
            {
                Requestify.Language = Requestify.ELanguage.SV;
            }
            else if (lang.Name.Contains("th"))
            {
                Requestify.Language = Requestify.ELanguage.TH;
            }
            else if (lang.Name.Contains("tr"))
            {
                Requestify.Language = Requestify.ELanguage.TR;
            }
            else if (lang.Name.Contains("uk"))
            {
                Requestify.Language = Requestify.ELanguage.UK;
            }
            else if (lang.Name.Contains("zh"))
            {
                if (lang.Name.Contains("CN"))
                {
                    Requestify.Language = Requestify.ELanguage.SZN;
                }
                else
                {
                    Requestify.Language = Requestify.ELanguage.TZN;
                }
            }
            else
            {
                Requestify.Language = Requestify.ELanguage.EN;
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
        public void UndefinedMessage_OnUndefinedMessage(RequestifyEventArgs.UndefinedMessageArgs e)
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
            if (AppConfig.CurrentConfig.Buttons.buttons.Count> id)
            {
                if (AppConfig.CurrentConfig.Buttons.buttons[id] != null)
                {
                    var b = AppConfig.CurrentConfig.Buttons.buttons[id];
                    if (b.BindType == "YoutubeMusic")
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
                        if ( AudioManager.Extra.SoundOut.PlaybackState == PlaybackState.Paused ||
                             AudioManager.Extra.SoundOut.PlaybackState == PlaybackState.Playing)
                        {
                             AudioManager.Extra.SoundOut.Stop();
                        }

                         AudioManager.Extra.SoundOut.Initialize(
                            new AacDecoder(streamInfo.Url).ToMono());
                         AudioManager.Extra.SoundOut.Play();

                    }

                    if (b.BindType == "LocalMusic")
                    {
                        if (File.Exists(AppConfig.CurrentConfig.Buttons.buttons[id].Link))
                        {
                            if ( AudioManager.Extra.SoundOut.PlaybackState == PlaybackState.Paused ||
                                 AudioManager.Extra.SoundOut.PlaybackState == PlaybackState.Playing)
                            {
                                 AudioManager.Extra.SoundOut.Stop();
                            }

                             AudioManager.Extra.SoundOut.Initialize(
                                new Mp3MediafoundationDecoder(AppConfig.CurrentConfig.Buttons.buttons[id].Link).ToMono());
                             AudioManager.Extra.SoundOut.Play();
                        }
                    }

                    if (b.BindType == "Stop")
                    {
                        if ( AudioManager.Extra.SoundOut.PlaybackState == PlaybackState.Paused ||
                             AudioManager.Extra.SoundOut.PlaybackState == PlaybackState.Playing)
                        {
                             AudioManager.Extra.SoundOut.Stop();
                        }
                    }

                }
            }
        }
        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            

            var plugins = PluginManager.GetPlugins();
            if (plugins.Count == 0)
            {
                Logger.Nlogger.Error(Application.Current.FindResource("cs_Cant_Find_Plugins").ToString());
            }
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}