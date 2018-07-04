using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Ookii.Dialogs;
using RequestifyTF2;
using RequestifyTF2.Api;
using RequestifyTF2.Utils;

namespace RequestifyTF2GUIRedone
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool _started;


        private TextWriter _writer;

        public MainWindow()
        {
            InitializeComponent();
            new Thread(StatsMonitor).Start();
        }

        private void StatsMonitor()
        {
            while (true)
            {
                LinesParsed.Dispatcher.Invoke(() => { LinesParsed.Content = Statisctics.LinesParsed.ToString(); });

                CommandsParsed.Dispatcher.Invoke(() =>
                {
                    CommandsParsed.Content = Statisctics.CommandsParsed.ToString();
                });
                GameKills.Dispatcher.Invoke(() => { GameKills.Content = Statisctics.GameKills.ToString(); });
                YourKills.Dispatcher.Invoke(() => { YourKills.Content = Statisctics.YourKills.ToString(); });
                TotalKills.Dispatcher.Invoke(() =>
                {
                    TotalKills.Content = Convert.ToString(Statisctics.YourKills + Statisctics.GameKills);
                });
                YourCritKills.Dispatcher.Invoke(() =>
                {
                    YourCritKills.Content = Statisctics.YourCritsKill.ToString();
                });
                GameCritKills.Dispatcher.Invoke(() => { GameCritKills.Content = Statisctics.GameKills.ToString(); });
                TotalCritKills.Dispatcher.Invoke(() =>
                {
                    TotalCritKills.Content = Convert.ToString(Statisctics.CritsKill + Statisctics.YourCritsKill);
                });
                YourDeaths.Dispatcher.Invoke(() => { YourDeaths.Content = Statisctics.YourDeaths.ToString(); });
                GameDeaths.Dispatcher.Invoke(() => { GameDeaths.Content = Statisctics.Deaths.ToString(); });
                TotalDeaths.Dispatcher.Invoke(() =>
                {
                    TotalDeaths.Content = Convert.ToString(Statisctics.YourDeaths + Statisctics.Deaths);
                });
                YourSuicides.Dispatcher.Invoke(() => { YourSuicides.Content = Statisctics.YourSuicides.ToString(); });
                GameSuicides.Dispatcher.Invoke(() => { GameSuicides.Content = Statisctics.Suicides.ToString(); });
                TotalSuicides.Dispatcher.Invoke(() =>
                {
                    TotalSuicides.Content = Convert.ToString(Statisctics.Suicides + Statisctics.YourSuicides);
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
                DragMove();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //LabelResp();
            //lbl.Content += "kek\n"
            if (Instance.Config.GameDir == string.Empty)
            {
                MessageBox.Show(
                    "Please set the game directory",
                    "Error"
                );

                return;
            }

            _started = Runner.Start();
            if (_started)
            {
                StartButton.Content = "Stop";
                StatusLabel.Content = "Status: Working";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var s = new VistaFolderBrowserDialog())
            {
                s.UseDescriptionForTitle = true;
                s.Description = "Select game folder";

                if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (s.SelectedPath == string.Empty)
                        return;

                    var dirs = Directory.GetDirectories(s.SelectedPath);

                    if (dirs.Any(n => n.Contains("cfg")))
                    {
                        AppConfig.CurrentConfig.GameDirectory = s.SelectedPath;
                        GamePath.Content = "Current game path: " + s.SelectedPath;
                        AppConfig.Save();
                    }
                    else
                    {
                        foreach (var dir in dirs)
                        {
                            var cdir = Directory.GetDirectories(dir);
                            var bin = false;
                            var cfg = false;
                            foreach (var dirz in cdir)
                            {
                                var pal = dirz;
                                var z = pal.Remove(0, dir.Length);

                                if (z.Contains("cfg")) cfg = true;

                                if (z.Contains("bin")) bin = true;

                                if (bin && cfg)
                                {
                                    AppConfig.CurrentConfig.GameDirectory = dir;
                                    MessageBox.Show(
                                        $"Game path was automatically corrected from \n{s.SelectedPath}\nto\n{dir}",
                                        "Done"
                                    );
                                    GamePath.Content = "Current game path: " + dir;
                                    AppConfig.Save();
                                    return;
                                }
                            }
                        }

                        MessageBox.Show(
                            "Cant find cfg folder.. \nMaybe its not a game folder? \nIf its CSGO pick 'csgo' folder, if TF2 pick 'tf2' folder, ect.",
                            "Error");
                    }
                }
            }
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            _writer = new TextBoxStreamWriter(Console, ConsoleLabel2);
            System.Console.SetOut(_writer);
            var plugins = Instance.Plugins.GetPlugins();
            if (plugins.Count == 0) Logger.Write(Logger.Status.Error, "I can't find any plugins");

            foreach (var item in plugins)
                PluginsList.Items.Add(new PluginItem {Plugin = item.plugin, PluginName = item.plugin.Name});

            foreach (var com in Instance.Commands.GetCommands())
                CommandsBox.Items.Add(new CommandItem {Command = com.ICommand, CommandName = com.Name});
            var a = Instance.Commands.GetCommands();
            AppConfig.Load();
            AdminName.Content = "Admin: " + Instance.Config.Admin;
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
                if (!Instance.Config.Ignored.Contains(IgnoreBox.Text)) Instance.Config.Ignored.Add(IgnoreBox.Text);
            }
        }

        private void IgnoreListButtonRemove_OnClick(object sender, RoutedEventArgs e)
        {
            if (IgnoreList.Items.Contains(IgnoreBox.Text))
            {
                if (Instance.Config.Ignored.Contains(IgnoreBox.Text)) Instance.Config.Ignored.Remove(IgnoreBox.Text);
                IgnoreList.Items.Remove(IgnoreBox.Text);
                return;
            }

            if (IgnoreList.SelectedItem != null)
            {
                if (Instance.Config.Ignored.Contains(IgnoreList.SelectedItem))
                    Instance.Config.Ignored.Remove(IgnoreList.SelectedItem.ToString());
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
    }
}