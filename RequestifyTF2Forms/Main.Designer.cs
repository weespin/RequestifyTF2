namespace RequestifyTF2Forms
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PluginsList = new System.Windows.Forms.CheckedListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnl_main = new System.Windows.Forms.Panel();
            this.btn_start = new System.Windows.Forms.Button();
            this.btn_mainshow = new System.Windows.Forms.Button();
            this.btn_ignorelistshow = new System.Windows.Forms.Button();
            this.btn_consoleshow = new System.Windows.Forms.Button();
            this.bnt_settingsshow = new System.Windows.Forms.Button();
            this.pnl_ignorelist = new System.Windows.Forms.Panel();
            this.chkbx_ListReversed = new System.Windows.Forms.CheckBox();
            this.btn_ListAdd = new System.Windows.Forms.Button();
            this.btn_ListRemove = new System.Windows.Forms.Button();
            this.tbx_ListToAdd = new System.Windows.Forms.TextBox();
            this.lbx_IgnoreList = new System.Windows.Forms.ListBox();
            this.pnl_Settings = new System.Windows.Forms.Panel();
            this.btn_SelectGamePath = new System.Windows.Forms.Button();
            this.txtbx_GamePath = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_howtouse = new System.Windows.Forms.Button();
            this.btn_helppanel = new System.Windows.Forms.Button();
            this.pnl_help = new System.Windows.Forms.Panel();
            this.pnl_main.SuspendLayout();
            this.pnl_ignorelist.SuspendLayout();
            this.pnl_Settings.SuspendLayout();
            this.pnl_help.SuspendLayout();
            this.SuspendLayout();
            // 
            // PluginsList
            // 
            this.PluginsList.FormattingEnabled = true;
            this.PluginsList.Location = new System.Drawing.Point(10, 3);
            this.PluginsList.Name = "PluginsList";
            this.PluginsList.Size = new System.Drawing.Size(120, 139);
            this.PluginsList.TabIndex = 1;
            this.PluginsList.SelectedIndexChanged += new System.EventHandler(this.PluginsList_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(146, 66);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Only with code";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(143, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "Code:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 203);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Weespin 2016";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // pnl_main
            // 
            this.pnl_main.Controls.Add(this.btn_start);
            this.pnl_main.Controls.Add(this.PluginsList);
            this.pnl_main.Controls.Add(this.label1);
            this.pnl_main.Controls.Add(this.checkBox1);
            this.pnl_main.Location = new System.Drawing.Point(12, 50);
            this.pnl_main.Name = "pnl_main";
            this.pnl_main.Size = new System.Drawing.Size(278, 150);
            this.pnl_main.TabIndex = 11;
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(146, 17);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(114, 23);
            this.btn_start.TabIndex = 7;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // btn_mainshow
            // 
            this.btn_mainshow.Location = new System.Drawing.Point(12, 3);
            this.btn_mainshow.Name = "btn_mainshow";
            this.btn_mainshow.Size = new System.Drawing.Size(65, 44);
            this.btn_mainshow.TabIndex = 12;
            this.btn_mainshow.Text = "Main";
            this.btn_mainshow.UseVisualStyleBackColor = true;
            this.btn_mainshow.Click += new System.EventHandler(this.btn_mainshow_Click);
            // 
            // btn_ignorelistshow
            // 
            this.btn_ignorelistshow.Location = new System.Drawing.Point(83, 3);
            this.btn_ignorelistshow.Name = "btn_ignorelistshow";
            this.btn_ignorelistshow.Size = new System.Drawing.Size(65, 44);
            this.btn_ignorelistshow.TabIndex = 13;
            this.btn_ignorelistshow.Text = "Ignore List";
            this.btn_ignorelistshow.UseVisualStyleBackColor = true;
            this.btn_ignorelistshow.Click += new System.EventHandler(this.btn_ignorelistshow_Click);
            // 
            // btn_consoleshow
            // 
            this.btn_consoleshow.Location = new System.Drawing.Point(10, 127);
            this.btn_consoleshow.Name = "btn_consoleshow";
            this.btn_consoleshow.Size = new System.Drawing.Size(114, 23);
            this.btn_consoleshow.TabIndex = 14;
            this.btn_consoleshow.Text = "Console";
            this.btn_consoleshow.UseVisualStyleBackColor = true;
            this.btn_consoleshow.Click += new System.EventHandler(this.btn_consoleshow_Click);
            // 
            // bnt_settingsshow
            // 
            this.bnt_settingsshow.Location = new System.Drawing.Point(154, 3);
            this.bnt_settingsshow.Name = "bnt_settingsshow";
            this.bnt_settingsshow.Size = new System.Drawing.Size(65, 44);
            this.bnt_settingsshow.TabIndex = 15;
            this.bnt_settingsshow.Text = "Settings";
            this.bnt_settingsshow.UseVisualStyleBackColor = true;
            this.bnt_settingsshow.Click += new System.EventHandler(this.bnt_settingsshow_Click);
            // 
            // pnl_ignorelist
            // 
            this.pnl_ignorelist.Controls.Add(this.btn_ListAdd);
            this.pnl_ignorelist.Controls.Add(this.chkbx_ListReversed);
            this.pnl_ignorelist.Controls.Add(this.btn_ListRemove);
            this.pnl_ignorelist.Controls.Add(this.tbx_ListToAdd);
            this.pnl_ignorelist.Controls.Add(this.lbx_IgnoreList);
            this.pnl_ignorelist.Location = new System.Drawing.Point(12, 50);
            this.pnl_ignorelist.Name = "pnl_ignorelist";
            this.pnl_ignorelist.Size = new System.Drawing.Size(278, 150);
            this.pnl_ignorelist.TabIndex = 16;
            // 
            // chkbx_ListReversed
            // 
            this.chkbx_ListReversed.AutoSize = true;
            this.chkbx_ListReversed.Location = new System.Drawing.Point(146, 91);
            this.chkbx_ListReversed.Name = "chkbx_ListReversed";
            this.chkbx_ListReversed.Size = new System.Drawing.Size(72, 17);
            this.chkbx_ListReversed.TabIndex = 23;
            this.chkbx_ListReversed.Text = "Reversed";
            this.chkbx_ListReversed.UseVisualStyleBackColor = true;
            this.chkbx_ListReversed.CheckedChanged += new System.EventHandler(this.chkbx_ListReversed_CheckedChanged);
            // 
            // btn_ListAdd
            // 
            this.btn_ListAdd.Location = new System.Drawing.Point(146, 62);
            this.btn_ListAdd.Name = "btn_ListAdd";
            this.btn_ListAdd.Size = new System.Drawing.Size(114, 23);
            this.btn_ListAdd.TabIndex = 22;
            this.btn_ListAdd.Text = "Add";
            this.btn_ListAdd.UseVisualStyleBackColor = true;
            this.btn_ListAdd.Click += new System.EventHandler(this.btn_ListAdd_Click);
            // 
            // btn_ListRemove
            // 
            this.btn_ListRemove.Location = new System.Drawing.Point(146, 6);
            this.btn_ListRemove.Name = "btn_ListRemove";
            this.btn_ListRemove.Size = new System.Drawing.Size(114, 23);
            this.btn_ListRemove.TabIndex = 21;
            this.btn_ListRemove.Text = "Remove";
            this.btn_ListRemove.UseVisualStyleBackColor = true;
            this.btn_ListRemove.Click += new System.EventHandler(this.btn_ListRemove_Click);
            // 
            // tbx_ListToAdd
            // 
            this.tbx_ListToAdd.Location = new System.Drawing.Point(146, 35);
            this.tbx_ListToAdd.Multiline = true;
            this.tbx_ListToAdd.Name = "tbx_ListToAdd";
            this.tbx_ListToAdd.Size = new System.Drawing.Size(114, 21);
            this.tbx_ListToAdd.TabIndex = 20;
            this.tbx_ListToAdd.Text = "Enter string";
            // 
            // lbx_IgnoreList
            // 
            this.lbx_IgnoreList.FormattingEnabled = true;
            this.lbx_IgnoreList.Location = new System.Drawing.Point(10, 3);
            this.lbx_IgnoreList.Name = "lbx_IgnoreList";
            this.lbx_IgnoreList.Size = new System.Drawing.Size(120, 134);
            this.lbx_IgnoreList.TabIndex = 17;
            // 
            // pnl_Settings
            // 
            this.pnl_Settings.Controls.Add(this.btn_SelectGamePath);
            this.pnl_Settings.Controls.Add(this.txtbx_GamePath);
            this.pnl_Settings.Controls.Add(this.btn_consoleshow);
            this.pnl_Settings.Location = new System.Drawing.Point(12, 50);
            this.pnl_Settings.Name = "pnl_Settings";
            this.pnl_Settings.Size = new System.Drawing.Size(278, 150);
            this.pnl_Settings.TabIndex = 17;
            // 
            // btn_SelectGamePath
            // 
            this.btn_SelectGamePath.Location = new System.Drawing.Point(146, 127);
            this.btn_SelectGamePath.Name = "btn_SelectGamePath";
            this.btn_SelectGamePath.Size = new System.Drawing.Size(118, 23);
            this.btn_SelectGamePath.TabIndex = 1;
            this.btn_SelectGamePath.Text = "Select Game Path";
            this.btn_SelectGamePath.UseVisualStyleBackColor = true;
            this.btn_SelectGamePath.Click += new System.EventHandler(this.btn_SelectGamePath_Click);
            // 
            // txtbx_GamePath
            // 
            this.txtbx_GamePath.BackColor = System.Drawing.SystemColors.Control;
            this.txtbx_GamePath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtbx_GamePath.Location = new System.Drawing.Point(20, 24);
            this.txtbx_GamePath.Multiline = true;
            this.txtbx_GamePath.Name = "txtbx_GamePath";
            this.txtbx_GamePath.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtbx_GamePath.Size = new System.Drawing.Size(240, 39);
            this.txtbx_GamePath.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox1.Location = new System.Drawing.Point(146, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(129, 98);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "RequestifyTF2 by Weespin\r\nThanks to:\r\nini - critical bug report\r\nd4rkc4t - autoho" +
    "tkey trick \r\n\r\n";
            // 
            // btn_howtouse
            // 
            this.btn_howtouse.Location = new System.Drawing.Point(10, 14);
            this.btn_howtouse.Name = "btn_howtouse";
            this.btn_howtouse.Size = new System.Drawing.Size(114, 23);
            this.btn_howtouse.TabIndex = 0;
            this.btn_howtouse.Text = "How to use";
            this.btn_howtouse.UseVisualStyleBackColor = true;
            this.btn_howtouse.Click += new System.EventHandler(this.btn_howtouse_Click);
            // 
            // btn_helppanel
            // 
            this.btn_helppanel.Location = new System.Drawing.Point(225, 3);
            this.btn_helppanel.Name = "btn_helppanel";
            this.btn_helppanel.Size = new System.Drawing.Size(65, 44);
            this.btn_helppanel.TabIndex = 18;
            this.btn_helppanel.Text = "Help";
            this.btn_helppanel.UseVisualStyleBackColor = true;
            this.btn_helppanel.Click += new System.EventHandler(this.btn_helppanel_Click);
            // 
            // pnl_help
            // 
            this.pnl_help.Controls.Add(this.textBox1);
            this.pnl_help.Controls.Add(this.btn_howtouse);
            this.pnl_help.Location = new System.Drawing.Point(12, 50);
            this.pnl_help.Name = "pnl_help";
            this.pnl_help.Size = new System.Drawing.Size(278, 150);
            this.pnl_help.TabIndex = 18;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 216);
            this.Controls.Add(this.btn_helppanel);
            this.Controls.Add(this.bnt_settingsshow);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnl_main);
            this.Controls.Add(this.btn_ignorelistshow);
            this.Controls.Add(this.btn_mainshow);
            this.Controls.Add(this.pnl_Settings);
            this.Controls.Add(this.pnl_help);
            this.Controls.Add(this.pnl_ignorelist);
            this.Name = "Main";
            this.Text = "RequestifyTF2";
            this.Load += new System.EventHandler(this.Main_Load);
            this.pnl_main.ResumeLayout(false);
            this.pnl_main.PerformLayout();
            this.pnl_ignorelist.ResumeLayout(false);
            this.pnl_ignorelist.PerformLayout();
            this.pnl_Settings.ResumeLayout(false);
            this.pnl_Settings.PerformLayout();
            this.pnl_help.ResumeLayout(false);
            this.pnl_help.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckedListBox PluginsList;
        private System.Windows.Forms.CheckBox checkBox1;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnl_main;
        private System.Windows.Forms.Button btn_mainshow;
        private System.Windows.Forms.Button btn_ignorelistshow;
        private System.Windows.Forms.Button btn_consoleshow;
        private System.Windows.Forms.Button bnt_settingsshow;
        private System.Windows.Forms.Panel pnl_ignorelist;
        private System.Windows.Forms.ListBox lbx_IgnoreList;
        private System.Windows.Forms.Panel pnl_Settings;
        private System.Windows.Forms.TextBox tbx_ListToAdd;
        private System.Windows.Forms.Button btn_ListRemove;
        private System.Windows.Forms.Button btn_ListAdd;
        private System.Windows.Forms.CheckBox chkbx_ListReversed;
        private System.Windows.Forms.Button btn_SelectGamePath;
        private System.Windows.Forms.TextBox txtbx_GamePath;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_howtouse;
        private System.Windows.Forms.Button btn_helppanel;
        private System.Windows.Forms.Panel pnl_help;
    }
}

