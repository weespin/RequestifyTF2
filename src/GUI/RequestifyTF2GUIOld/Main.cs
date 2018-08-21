// RequestifyTF2GUIOld(unsupported)
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ConsoleRedirection;
using MaterialSkin;
using MaterialSkin.Controls;
using Ookii.Dialogs;
using RequestifyTF2;
using RequestifyTF2.API;
using RequestifyTF2.API.IgnoreList;
using RequestifyTF2.Managers;
using RequestifyTF2Forms.Config;
using RequestifyTF2GUIOld.Properties;

namespace RequestifyTF2Forms
{
    public partial class Main : MaterialForm
    {
        public static bool ConsoleShowed;

        public static Main instance;

        private readonly Dictionary<string, PluginManager.Plugin> _plugins;

        private readonly Console cs = new Console();

        private bool _started;

        private TextWriter _writer;

        public Main()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.BlueGrey800,
                Primary.BlueGrey900,
                Primary.BlueGrey500,
                Accent.LightBlue200,
                TextShade.WHITE);
            instance = this;
            Icon = Resources.Icon;
            _plugins = new Dictionary<string, PluginManager.Plugin>();
            if (!Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/"))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/");
            }

            MaximizeBox = false;
            field_ignored.Enter += lbx_IgnoreList_Enter;

            var plugins = PluginManager.GetPlugins();
            foreach (var item in plugins)
            {
                _plugins.Add(item.plugin.Name, item);
            }

            seedListView(plugins);
            AppConfig.Load();
            materialSingleLineTextField1.Text = Requestify.Admin;
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var toignorenick = field_ignored.Text;

            if (toignorenick == string.Empty || IgnoreList.Contains(toignorenick))
            {
                return;
            }

            var namencommand = new[] {toignorenick};
            var items = new ListViewItem(namencommand);
            list_ignored.Items.Add(items);
            IgnoreList.Add(toignorenick);
        }

        private void btn_consoleshow_Click_1(object sender, EventArgs e)
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

        private void btn_remove_Click(object sender, EventArgs e)
        {
            if (list_ignored.SelectedItems.Count == 0)
            {
                return;
            }

            var selected = list_ignored.SelectedItems[0];

            if (selected != null)
            {
                IgnoreList.Remove(selected.ToString());
                list_ignored.Items.Remove(selected);
            }
        }

        private void btn_SelectGamePath_Click_1(object sender, EventArgs e)
        {
            using (var s = new VistaFolderBrowserDialog())
            {
                s.UseDescriptionForTitle = true;
                s.Description = "Select game folder";

                if (s.ShowDialog() == DialogResult.OK)
                {
                    if (s.SelectedPath == string.Empty)
                    {
                        return;
                    }

                    var dirs = Directory.GetDirectories(s.SelectedPath);

                    if (dirs.Any(n => n.Contains("cfg")))
                    {
                        AppConfig.CurrentConfig.GameDirectory = s.SelectedPath;
                        txtbx_GamePath.Text = "Current game path: " + s.SelectedPath;
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

                                if (z.Contains("cfg"))
                                {
                                    cfg = true;
                                }

                                if (z.Contains("bin"))
                                {
                                    bin = true;
                                }

                                if (bin && cfg)
                                {
                                    AppConfig.CurrentConfig.GameDirectory = dir;
                                    new RequestifyTF2GUI.MessageBox.MessageBox().Show(
                                        $"Game path was automatically corrected from \n{s.SelectedPath}\nto\n{dir}",
                                        "Done",
                                        RequestifyTF2GUI.MessageBox.MessageBox.Sounds.Exclamation);
                                    txtbx_GamePath.Text = "Current game path: " + dir;
                                    AppConfig.Save();
                                    return;
                                }
                            }
                        }

                        new RequestifyTF2GUI.MessageBox.MessageBox().Show(
                            "Cant find cfg folder.. \nMaybe its not a game folder? \nIf its CSGO pick 'csgo' folder, if TF2 pick 'tf2' folder, ect.",
                            "Error",
                            RequestifyTF2GUI.MessageBox.MessageBox.Sounds.Exclamation);
                    }
                }
            }
        }

        private void btn_start_Click_1(object sender, EventArgs e)
        {
            if (Requestify.GameDir == string.Empty)
            {
                new RequestifyTF2GUI.MessageBox.MessageBox().Show(
                    "Please set the game directory",
                    "Error",
                    RequestifyTF2GUI.MessageBox.MessageBox.Sounds.Exclamation);

                return;
            }

            if (!_started)
            {
                Runner.Start();
                _started = true;

                var s = _plugins.Aggregate(string.Empty, (current, plugin) => current + plugin.Value.plugin.Name);
                btn_start.Text = "Started";
            }

            btn_start.Enabled = false;
            materialLabel5.Text = "Status: Working";
        }

        private void lbx_IgnoreList_Enter(object sender, EventArgs e)
        {
            if (field_ignored.Text == "Enter Name")
            {
                field_ignored.Text = string.Empty;
            }
        }

        private void list_plugins_MouseClick(object sender, MouseEventArgs e)
        {
            var me = e;
            if ((me.Button & MouseButtons.Right) != 0)
            {
                var msgbox = new MessageBox();
                if (list_plugins.SelectedItems[0].SubItems[0].Text != string.Empty
                    || list_plugins.SelectedItems.Count != 0)
                {
                    msgbox.MessageText = _plugins[list_plugins.SelectedItems[0].SubItems[0].Text].plugin.Desc;

                    msgbox.Text = list_plugins.SelectedItems[0].SubItems[0].Text;
                    msgbox.Show();
                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            txtbx_GamePath.Text = "Current game path: " + AppConfig.CurrentConfig.GameDirectory;

            // Instantiate the writer
            _writer = new TextBoxStreamWriter(cs.txt_console);

            // Redirect the out Console stream
            System.Console.SetOut(_writer);
        }

        private void materialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            IgnoreList.Reversed = chkbox_reverse.Checked;
        }

        private void materialLabel1_Click(object sender, EventArgs e)
        {
            Process.Start("https://steamcommunity.com/id/wspin/");
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            Requestify.Admin = materialSingleLineTextField1.Text;
            AppConfig.CurrentConfig.Admin = materialSingleLineTextField1.Text;
            AppConfig.Save();
        }


        private void seedListView(ICollection<PluginManager.Plugin> plugins)
        {
            // Define
            foreach (var item in plugins)
            {
                var namencommand = new[] {item.plugin.Name, item.plugin.Desc, item.plugin.Author, "True"};
                var items = new ListViewItem(namencommand);
                list_plugins.Items.Add(items);
            }
        }

        private void materialCheckBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            Requestify.IsMuted = materialCheckBox1.Checked;
        }
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
    }
}