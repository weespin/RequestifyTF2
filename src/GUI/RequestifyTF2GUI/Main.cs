using RequestifyTF2.Managers;

namespace RequestifyTF2Forms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    using ConsoleRedirection;

    using MaterialSkin;
    using MaterialSkin.Controls;

    using Ookii.Dialogs;

    using RequestifyTF2;
    using RequestifyTF2.Api;

    using RequestifyTF2Forms.Config;

    using RequestifyTF2GUI.Properties;

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
            this.InitializeComponent();

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
            this.Icon = Resources.Icon;
            this._plugins = new Dictionary<string, PluginManager.Plugin>();
            if (!Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/"))
                Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/");

            this.MaximizeBox = false;
            this.field_ignored.Enter += this.lbx_IgnoreList_Enter;
          Instance.Plugins.loadPlugins(
                    Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/");
            var plugins = Instance.Plugins.GetPlugins();
            foreach (var item in plugins)
            {
              //  Instance.ActivePlugins.Add(item);
                this._plugins.Add(item.plugin.Name, item);

                // PluginsList.Items.Add(item.Name, true);
            }

            this.seedListView(plugins);
            this.FormClosing += this.Main_Closing;
            AppConfig.Load();
            this.materialSingleLineTextField1.Text = Instance.Config.Admin;
            new Thread(
                () =>
                    {
                        Thread.CurrentThread.IsBackground = true;

                        while (true)
                        {
                            Thread.Sleep(2000);

                            ThreadHelperClass.SetText(this, this.lbl_code, "Code: " + Instance.Config.Chiper);
                        }
                    }).Start();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var toignorenick = this.field_ignored.Text;

            if (toignorenick == string.Empty || Instance.Config.Ignored.Contains(toignorenick)) return;

            var namencommand = new[] { toignorenick };
            var items = new ListViewItem(namencommand);
            this.list_ignored.Items.Add(items);
            Instance.Config.Ignored.Add(toignorenick);
        }

        private void btn_consoleshow_Click_1(object sender, EventArgs e)
        {
            if (!ConsoleShowed)
            {
                this.cs.Show();
                ConsoleShowed = true;
            }
            else
            {
                this.cs.Hide();
                ConsoleShowed = false;
            }
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {
            if (this.list_ignored.SelectedItems.Count == 0) return;
            var selected = this.list_ignored.SelectedItems[0];

            if (selected != null)
            {
                Instance.Config.Ignored.Remove(selected.ToString());
                this.list_ignored.Items.Remove(selected);
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
                        return;

                    var dirs = Directory.GetDirectories(s.SelectedPath);

                    if (dirs.Any(n => n.Contains("cfg")))
                    {
                        AppConfig.CurrentConfig.GameDirectory = s.SelectedPath;
                        this.txtbx_GamePath.Text = "Current game path: " + s.SelectedPath;
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
                                    this.txtbx_GamePath.Text = "Current game path: " + dir;
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
            if (Instance.Config.GameDir == string.Empty)
            {
                new RequestifyTF2GUI.MessageBox.MessageBox().Show(
                    "Please set the game directory",
                    "Error",
                    RequestifyTF2GUI.MessageBox.MessageBox.Sounds.Exclamation);

                return;
            }

            if (!this._started)
            {
                Runner.Start();
                this._started = true;

                var s = this._plugins.Aggregate(string.Empty, (current, plugin) => current + plugin.Value.plugin.Name);
            }

            this.btn_start.Enabled = false;
            this.materialLabel5.Text = "Status: Working";
        }

        private void chkbox_onlywithcode_CheckedChanged(object sender, EventArgs e)
        {
         //   Instance.Config.OnlyWithCode = this.chkbox_onlywithcode.Checked;
        }

        private void lbx_IgnoreList_Enter(object sender, EventArgs e)
        {
            if (this.field_ignored.Text == "Enter Name") this.field_ignored.Text = string.Empty;
        }

        private void list_plugins_DoubleClick(object sender, EventArgs e)
        {
            //if (this.list_plugins.SelectedItems[0].SubItems[3].Text == "True")
            //{
            //    this.list_plugins.SelectedItems[0].SubItems[3].Text = "False";
            //    foreach (var s in Instance.ActivePlugins)
            //        if (s.Name == this.list_plugins.SelectedItems[0].SubItems[0].Text)
            //        {
            //            Instance.ActivePlugins.Remove(s);
            //            Instance.DisabledPlugins.Add(s);
            //            break;
            //        }
            //}
            //else
            //{
            //    this.list_plugins.SelectedItems[0].SubItems[3].Text = "True";
            //    foreach (var s in Instance.DisabledPlugins)
            //        if (s.Name == this.list_plugins.SelectedItems[0].SubItems[0].Text)
            //        {
            //            Instance.ActivePlugins.Add(s);
            //            Instance.DisabledPlugins.Remove(s);
            //            break;
            //        }
            //}
        }

        private void list_plugins_MouseClick(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = e;
            if ((me.Button & MouseButtons.Right) != 0)
            {
                var msgbox = new MessageBox();
                if (this.list_plugins.SelectedItems[0].SubItems[0].Text != string.Empty
                    || this.list_plugins.SelectedItems.Count != 0)
                {
                    msgbox.MessageText = this._plugins[this.list_plugins.SelectedItems[0].SubItems[0].Text].plugin.Desc;

                    msgbox.Text = this.list_plugins.SelectedItems[0].SubItems[0].Text;
                    msgbox.Show();
                }

                // MessageBox.Show(selected.SubItems[0].Text);
            }
        }

        private void list_plugins_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Main_Closing(object sender, EventArgs e)
        {
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.txtbx_GamePath.Text = "Current game path: " + AppConfig.CurrentConfig.GameDirectory;

            // Instantiate the writer
            this._writer = new TextBoxStreamWriter(this.cs.txt_console);

            // Redirect the out Console stream
            System.Console.SetOut(this._writer);
        }

        private void materialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            Instance.Config.IgnoredReversed = this.chkbox_reverse.Checked;
        }

        private void materialLabel1_Click(object sender, EventArgs e)
        {
            Process.Start("https://steamcommunity.com/id/wspin/");
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            Instance.Config.Admin = AppConfig.CurrentConfig.Admin = this.materialSingleLineTextField1.Text;
            AppConfig.Save();
        }

        private void materialSingleLineTextField1_Click(object sender, EventArgs e)
        {
        }

        private void materialSingleLineTextField1_DragLeave(object sender, EventArgs e)
        {
        }

        private void seedListView(ICollection<PluginManager.Plugin> plugins)
        {
            // Define
            foreach (var item in plugins)
            {
                var namencommand = new[] { item.plugin.Name, item.plugin.Desc, item.plugin.Author, "True" };
                var items = new ListViewItem(namencommand);
                this.list_plugins.Items.Add(items);
            }

            /*  foreach (var in data)
              {
                  var item = new ListViewItem(version);
                  materialListView1.Items.Add(item);
              }*/
        }
    }

    public static class ThreadHelperClass
    {
        private delegate void SetTextCallback(Form f, Control ctrl, string text);

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
    }
}