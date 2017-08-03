using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using ConsoleRedirection;
using Ookii.Dialogs;
using RequestifyTF2;
using RequestifyTF2.Api;
using RequestifyTF2Forms.Config;
using RequestifyTF2Forms.Properties;

namespace RequestifyTF2Forms
{
    public partial class Main : Form
    {
        private static bool _started;
        public static Main instance;
        public static bool ConsoleShowed;
        private readonly Dictionary<string, IRequestifyPlugin> _plugins;
        private readonly Console cs = new Console();
        private TextWriter _writer;

        public Main()
        {
            InitializeComponent();
            instance = this;
            Icon = Resources._1481916367_letter_r_red;
            _plugins = new Dictionary<string, IRequestifyPlugin>();
            if (!Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/")
            )
                Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/");
            Instance.Config.AhkPath = Path.GetDirectoryName(Application.ExecutablePath) +
                                       "/plugins/ahk/ahk.exe";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            tbx_ListToAdd.Enter += lbx_IgnoreList_Enter;
            var plugins =
                PluginLoader<IRequestifyPlugin>.LoadPlugins(Path.GetDirectoryName(Application.ExecutablePath) +
                                                            "/plugins/");
            foreach (var item in plugins)
            {
                Instance.ActivePlugins.Add(item);
                _plugins.Add(item.Name, item);
                PluginsList.Items.Add(item.Name, true);
            }

            FormClosing += Main_Closing;
            AppConfig.Load();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                while (true)
                {
                    Thread.Sleep(2000);

                    ThreadHelperClass.SetText(this, label1, "Code: " + Instance.Config.Chiper);
                }
            }).Start();
        }

        #region Ect

        private void label2_Click(object sender, EventArgs e)
        {
            Process.Start("https://steamcommunity.com/id/wspin/");
        }

        #endregion

        #region SettingsPanel

        private void btn_SelectGamePath_Click(object sender, EventArgs e)
        {
            using (var s = new VistaFolderBrowserDialog())
            {
                s.UseDescriptionForTitle = true;
                s.Description = "Select game folder";

                if (s.ShowDialog() == DialogResult.OK)
                {
                    if (s.SelectedPath == "")
                        return;
                    Instance.Config.GameDir = s.SelectedPath;
                    txtbx_GamePath.Text = "Current game path: " + Instance.Config.GameDir;
                    var dirs = Directory.GetDirectories(s.SelectedPath);
                    if (dirs.Any(n => n.Contains("cfg")))
                    {
                    }
                    else
                    {
                        MessageBox.Show(
                            "Cant find cfg folder.. \nMaybe its not a game folder? \nIf its CSGO pick 'csgo' folder, if TF2 pick 'tf2' folder.");
                        s.SelectedPath = "";
                    }
                    AppConfig.CurrentConfig.GameDirectory = s.SelectedPath;
                    AppConfig.Save();
                }
            }
        }

        #endregion


      


        #region FormEvents

        private void Main_Load(object sender, EventArgs e)
        {
            txtbx_GamePath.Text = "Current game path: " + AppConfig.CurrentConfig.GameDirectory;
            // Instantiate the writer
            _writer = new TextBoxStreamWriter(cs.txt_console);
            // Redirect the out Console stream
            System.Console.SetOut(_writer);
        }


        private void Main_Closing(object sender, EventArgs e)
        {
            var vlcProcesses = Process.GetProcessesByName("vlc");
            foreach (var k in vlcProcesses)
                k.Kill();
        }

        #endregion

        #region IgnoreListPanel

        private void lbx_IgnoreList_Enter(object sender, EventArgs e)
        {
            if (tbx_ListToAdd.Text == "Enter string")
                tbx_ListToAdd.Text = "";
        }

        private void btn_ListRemove_Click(object sender, EventArgs e)
        {
            var selected = lbx_IgnoreList.SelectedItem;
            if (selected != null)
                Instance.Config.Ignored.Remove(selected.ToString());
            lbx_IgnoreList.Items.Remove(selected);
        }

        private void btn_ListAdd_Click(object sender, EventArgs e)
        {
            var toignorenick = tbx_ListToAdd.Text;

            lbx_IgnoreList.Items.Add(toignorenick);
            Instance.Config.Ignored.Add(toignorenick);
        }

        private void chkbx_ListReversed_CheckedChanged(object sender, EventArgs e)
        {
            Instance.Config.IgnoredReversed = chkbx_ListReversed.Checked;
        }

        #endregion

        #region MainPanel

        private void PluginsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = PluginsList.SelectedIndex;
            if (selected == -1)
                return;
            if (PluginsList.GetItemCheckState(selected) == CheckState.Checked)
                foreach (var s in Instance.DisabledPlugins)
                    if (s.Name == PluginsList.Items[selected].ToString())
                    {
                        Instance.ActivePlugins.Add(s);
                        Instance.DisabledPlugins.Remove(s);
                        break;
                    }

            if (PluginsList.GetItemCheckState(selected) == CheckState.Unchecked)
                foreach (var s in Instance.ActivePlugins)
                    if (s.Name == PluginsList.Items[selected].ToString())
                    {
                        Instance.ActivePlugins.Remove(s);
                        Instance.DisabledPlugins.Add(s);
                        break;
                    }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (!_started)
                Runner.Start();
            btn_start.Enabled = false;
            _started = true;

            var s = _plugins.Aggregate("", (current, plugin) => current + (plugin.Value.Name));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Instance.Config.OnlyWithCode = btn_onlycode.Checked;
        }

        #endregion

        #region  controlbuttons       

        private void btn_consoleshow_Click(object sender, EventArgs e)
        {
            if (!ConsoleShowed)
            {
                cs.Show();
                ConsoleShowed = true;
            }
            else
            {
                cs.Hide();
                ConsoleShowed = false;
            }
        }

        private void btn_ignorelistshow_Click(object sender, EventArgs e)
        {
            pnl_ignorelist.Visible = true;
            pnl_main.Visible = false;
            pnl_Settings.Visible = false;
           
        }

        private void btn_mainshow_Click(object sender, EventArgs e)
        {
            pnl_ignorelist.Visible = false;
            pnl_main.Visible = true;
            pnl_Settings.Visible = false;
           
        }

        private void bnt_settingsshow_Click(object sender, EventArgs e)
        {
            pnl_ignorelist.Visible = false;
            pnl_main.Visible = false;
            pnl_Settings.Visible = true;

        }


        #endregion
    }

    public static class ThreadHelperClass
    {
        /// <summary>
        ///     Set text property of various controls
        /// </summary>
        /// <param name="form">The calling form</param>
        /// <param name="ctrl"></param>
        /// <param name="text"></param>
        public static void SetText(Form form, Control ctrl, string text)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (ctrl.InvokeRequired)
            {
                SetTextCallback d = SetText;
                try
                {
                    form.Invoke(d, form, ctrl, text);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            else
            {
                ctrl.Text = text;
            }
        }

        private delegate void SetTextCallback(Form f, Control ctrl, string text);

        private delegate void SetPosCallback(Form f, Control ctrl, Point p);
    }
}