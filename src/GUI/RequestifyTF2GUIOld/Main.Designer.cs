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
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.materialLabel5 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel4 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialDivider1 = new MaterialSkin.Controls.MaterialDivider();
            this.btn_start = new MaterialSkin.Controls.MaterialRaisedButton();
            this.chkbox_onlywithcode = new MaterialSkin.Controls.MaterialCheckBox();
            this.lbl_code = new MaterialSkin.Controls.MaterialLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.materialCheckBox1 = new MaterialSkin.Controls.MaterialCheckBox();
            this.materialRaisedButton1 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.materialLabel6 = new MaterialSkin.Controls.MaterialLabel();
            this.materialSingleLineTextField1 = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.btn_SelectGamePath = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btn_consoleshow = new MaterialSkin.Controls.MaterialRaisedButton();
            this.txtbx_GamePath = new MaterialSkin.Controls.MaterialLabel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.list_plugins = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.chkbox_reverse = new MaterialSkin.Controls.MaterialCheckBox();
            this.field_ignored = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.btn_add = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btn_remove = new MaterialSkin.Controls.MaterialRaisedButton();
            this.list_ignored = new MaterialSkin.Controls.MaterialListView();
            this.Player = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.materialTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Controls.Add(this.tabPage1);
            this.materialTabControl1.Controls.Add(this.tabPage2);
            this.materialTabControl1.Controls.Add(this.tabPage3);
            this.materialTabControl1.Controls.Add(this.tabPage4);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.materialTabControl1.Location = new System.Drawing.Point(0, 114);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(419, 254);
            this.materialTabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.materialLabel5);
            this.tabPage1.Controls.Add(this.materialLabel4);
            this.tabPage1.Controls.Add(this.materialLabel3);
            this.tabPage1.Controls.Add(this.materialLabel2);
            this.tabPage1.Controls.Add(this.materialLabel1);
            this.tabPage1.Controls.Add(this.materialDivider1);
            this.tabPage1.Controls.Add(this.btn_start);
            this.tabPage1.Controls.Add(this.chkbox_onlywithcode);
            this.tabPage1.Controls.Add(this.lbl_code);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(411, 228);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // materialLabel5
            // 
            this.materialLabel5.AutoSize = true;
            this.materialLabel5.Depth = 0;
            this.materialLabel5.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel5.Location = new System.Drawing.Point(256, 45);
            this.materialLabel5.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel5.Name = "materialLabel5";
            this.materialLabel5.Size = new System.Drawing.Size(100, 19);
            this.materialLabel5.TabIndex = 22;
            this.materialLabel5.Text = "Status: Ready";
            // 
            // materialLabel4
            // 
            this.materialLabel4.AutoSize = true;
            this.materialLabel4.Depth = 0;
            this.materialLabel4.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel4.Location = new System.Drawing.Point(6, 227);
            this.materialLabel4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            this.materialLabel4.Size = new System.Drawing.Size(185, 19);
            this.materialLabel4.TabIndex = 21;
            this.materialLabel4.Text = "Thanks to: ini, nullifiedcat ";
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel3.Location = new System.Drawing.Point(23, 101);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(366, 76);
            this.materialLabel3.TabIndex = 20;
            this.materialLabel3.Text = "Welcome to RequestifyTF2, press Start to launch bot.\r\nYou should press Start befo" +
    "re launching a game!\r\nDo not forget to specify the path in the settings!\r\n\r\n";
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(8, 3);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(0, 19);
            this.materialLabel2.TabIndex = 19;
            // 
            // materialLabel1
            // 
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(296, 227);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(115, 22);
            this.materialLabel1.TabIndex = 18;
            this.materialLabel1.Text = "Weespin 2018";
            this.materialLabel1.Click += new System.EventHandler(this.materialLabel1_Click);
            // 
            // materialDivider1
            // 
            this.materialDivider1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialDivider1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialDivider1.Depth = 0;
            this.materialDivider1.Location = new System.Drawing.Point(0, 192);
            this.materialDivider1.Margin = new System.Windows.Forms.Padding(0);
            this.materialDivider1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider1.Name = "materialDivider1";
            this.materialDivider1.Size = new System.Drawing.Size(408, 1);
            this.materialDivider1.TabIndex = 17;
            this.materialDivider1.Text = "materialDivider1";
            // 
            // btn_start
            // 
            this.btn_start.AutoSize = true;
            this.btn_start.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_start.Depth = 0;
            this.btn_start.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btn_start.Icon = null;
            this.btn_start.Location = new System.Drawing.Point(27, 6);
            this.btn_start.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_start.Name = "btn_start";
            this.btn_start.Primary = true;
            this.btn_start.Size = new System.Drawing.Size(64, 36);
            this.btn_start.TabIndex = 16;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click_1);
            // 
            // chkbox_onlywithcode
            // 
            this.chkbox_onlywithcode.AutoSize = true;
            this.chkbox_onlywithcode.Depth = 0;
            this.chkbox_onlywithcode.Font = new System.Drawing.Font("Roboto", 10F);
            this.chkbox_onlywithcode.Location = new System.Drawing.Point(27, 56);
            this.chkbox_onlywithcode.Margin = new System.Windows.Forms.Padding(0);
            this.chkbox_onlywithcode.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkbox_onlywithcode.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkbox_onlywithcode.Name = "chkbox_onlywithcode";
            this.chkbox_onlywithcode.Ripple = true;
            this.chkbox_onlywithcode.Size = new System.Drawing.Size(122, 30);
            this.chkbox_onlywithcode.TabIndex = 15;
            this.chkbox_onlywithcode.Text = "Only with Code";
            this.chkbox_onlywithcode.UseVisualStyleBackColor = true;
            // 
            // lbl_code
            // 
            this.lbl_code.AutoSize = true;
            this.lbl_code.Depth = 0;
            this.lbl_code.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_code.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_code.Location = new System.Drawing.Point(256, 14);
            this.lbl_code.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_code.Name = "lbl_code";
            this.lbl_code.Size = new System.Drawing.Size(122, 19);
            this.lbl_code.TabIndex = 13;
            this.lbl_code.Text = "Code: generating";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.materialCheckBox1);
            this.tabPage2.Controls.Add(this.materialRaisedButton1);
            this.tabPage2.Controls.Add(this.materialLabel6);
            this.tabPage2.Controls.Add(this.materialSingleLineTextField1);
            this.tabPage2.Controls.Add(this.btn_SelectGamePath);
            this.tabPage2.Controls.Add(this.btn_consoleshow);
            this.tabPage2.Controls.Add(this.txtbx_GamePath);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(411, 228);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Settings";
            // 
            // materialCheckBox1
            // 
            this.materialCheckBox1.AutoSize = true;
            this.materialCheckBox1.Depth = 0;
            this.materialCheckBox1.Font = new System.Drawing.Font("Roboto", 10F);
            this.materialCheckBox1.Location = new System.Drawing.Point(12, 101);
            this.materialCheckBox1.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckBox1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckBox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckBox1.Name = "materialCheckBox1";
            this.materialCheckBox1.Ripple = true;
            this.materialCheckBox1.Size = new System.Drawing.Size(69, 30);
            this.materialCheckBox1.TabIndex = 23;
            this.materialCheckBox1.Text = "Muted";
            this.materialCheckBox1.UseVisualStyleBackColor = true;
            this.materialCheckBox1.CheckedChanged += new System.EventHandler(this.materialCheckBox1_CheckedChanged_1);
            // 
            // materialRaisedButton1
            // 
            this.materialRaisedButton1.AutoSize = true;
            this.materialRaisedButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialRaisedButton1.Depth = 0;
            this.materialRaisedButton1.Icon = null;
            this.materialRaisedButton1.Location = new System.Drawing.Point(265, 64);
            this.materialRaisedButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButton1.Name = "materialRaisedButton1";
            this.materialRaisedButton1.Primary = true;
            this.materialRaisedButton1.Size = new System.Drawing.Size(55, 36);
            this.materialRaisedButton1.TabIndex = 22;
            this.materialRaisedButton1.Text = "Save";
            this.materialRaisedButton1.UseVisualStyleBackColor = true;
            this.materialRaisedButton1.Click += new System.EventHandler(this.materialRaisedButton1_Click);
            // 
            // materialLabel6
            // 
            this.materialLabel6.AutoSize = true;
            this.materialLabel6.Depth = 0;
            this.materialLabel6.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel6.Location = new System.Drawing.Point(27, 72);
            this.materialLabel6.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel6.Name = "materialLabel6";
            this.materialLabel6.Size = new System.Drawing.Size(100, 19);
            this.materialLabel6.TabIndex = 21;
            this.materialLabel6.Text = "Steam Name:";
            // 
            // materialSingleLineTextField1
            // 
            this.materialSingleLineTextField1.Depth = 0;
            this.materialSingleLineTextField1.Hint = "";
            this.materialSingleLineTextField1.Location = new System.Drawing.Point(133, 68);
            this.materialSingleLineTextField1.MaxLength = 32767;
            this.materialSingleLineTextField1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialSingleLineTextField1.Name = "materialSingleLineTextField1";
            this.materialSingleLineTextField1.PasswordChar = '\0';
            this.materialSingleLineTextField1.SelectedText = "";
            this.materialSingleLineTextField1.SelectionLength = 0;
            this.materialSingleLineTextField1.SelectionStart = 0;
            this.materialSingleLineTextField1.Size = new System.Drawing.Size(117, 23);
            this.materialSingleLineTextField1.TabIndex = 20;
            this.materialSingleLineTextField1.TabStop = false;
            this.materialSingleLineTextField1.UseSystemPasswordChar = false;
            // 
            // btn_SelectGamePath
            // 
            this.btn_SelectGamePath.AutoSize = true;
            this.btn_SelectGamePath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_SelectGamePath.Depth = 0;
            this.btn_SelectGamePath.Icon = null;
            this.btn_SelectGamePath.Location = new System.Drawing.Point(218, 16);
            this.btn_SelectGamePath.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_SelectGamePath.Name = "btn_SelectGamePath";
            this.btn_SelectGamePath.Primary = true;
            this.btn_SelectGamePath.Size = new System.Drawing.Size(151, 36);
            this.btn_SelectGamePath.TabIndex = 19;
            this.btn_SelectGamePath.Text = "Select Game Path";
            this.btn_SelectGamePath.UseVisualStyleBackColor = true;
            this.btn_SelectGamePath.Click += new System.EventHandler(this.btn_SelectGamePath_Click_1);
            // 
            // btn_consoleshow
            // 
            this.btn_consoleshow.AutoSize = true;
            this.btn_consoleshow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_consoleshow.Depth = 0;
            this.btn_consoleshow.Icon = null;
            this.btn_consoleshow.Location = new System.Drawing.Point(45, 16);
            this.btn_consoleshow.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_consoleshow.Name = "btn_consoleshow";
            this.btn_consoleshow.Primary = true;
            this.btn_consoleshow.Size = new System.Drawing.Size(82, 36);
            this.btn_consoleshow.TabIndex = 18;
            this.btn_consoleshow.Text = "Console";
            this.btn_consoleshow.UseVisualStyleBackColor = true;
            this.btn_consoleshow.Click += new System.EventHandler(this.btn_consoleshow_Click_1);
            // 
            // txtbx_GamePath
            // 
            this.txtbx_GamePath.Depth = 0;
            this.txtbx_GamePath.Font = new System.Drawing.Font("Roboto", 11F);
            this.txtbx_GamePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbx_GamePath.Location = new System.Drawing.Point(8, 137);
            this.txtbx_GamePath.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtbx_GamePath.Name = "txtbx_GamePath";
            this.txtbx_GamePath.Size = new System.Drawing.Size(394, 86);
            this.txtbx_GamePath.TabIndex = 15;
            this.txtbx_GamePath.Text = "materialLabel1";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.list_plugins);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(411, 228);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Plugins";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // list_plugins
            // 
            this.list_plugins.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.list_plugins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.list_plugins.Depth = 0;
            this.list_plugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list_plugins.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.list_plugins.FullRowSelect = true;
            this.list_plugins.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.list_plugins.Location = new System.Drawing.Point(0, 0);
            this.list_plugins.MouseLocation = new System.Drawing.Point(-1, -1);
            this.list_plugins.MouseState = MaterialSkin.MouseState.OUT;
            this.list_plugins.Name = "list_plugins";
            this.list_plugins.OwnerDraw = true;
            this.list_plugins.Size = new System.Drawing.Size(411, 228);
            this.list_plugins.TabIndex = 2;
            this.list_plugins.UseCompatibleStateImageBehavior = false;
            this.list_plugins.View = System.Windows.Forms.View.Details;
            this.list_plugins.MouseClick += new System.Windows.Forms.MouseEventHandler(this.list_plugins_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Plugin";
            this.columnHeader1.Width = 105;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Info";
            this.columnHeader2.Width = 110;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Author";
            this.columnHeader3.Width = 110;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Enabled";
            this.columnHeader4.Width = 89;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.White;
            this.tabPage4.Controls.Add(this.chkbox_reverse);
            this.tabPage4.Controls.Add(this.field_ignored);
            this.tabPage4.Controls.Add(this.btn_add);
            this.tabPage4.Controls.Add(this.btn_remove);
            this.tabPage4.Controls.Add(this.list_ignored);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(411, 228);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Ignore List";
            // 
            // chkbox_reverse
            // 
            this.chkbox_reverse.AutoSize = true;
            this.chkbox_reverse.Depth = 0;
            this.chkbox_reverse.Font = new System.Drawing.Font("Roboto", 10F);
            this.chkbox_reverse.Location = new System.Drawing.Point(253, 102);
            this.chkbox_reverse.Margin = new System.Windows.Forms.Padding(0);
            this.chkbox_reverse.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkbox_reverse.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkbox_reverse.Name = "chkbox_reverse";
            this.chkbox_reverse.Ripple = true;
            this.chkbox_reverse.Size = new System.Drawing.Size(87, 30);
            this.chkbox_reverse.TabIndex = 29;
            this.chkbox_reverse.Text = "Reversed";
            this.chkbox_reverse.UseVisualStyleBackColor = true;
            this.chkbox_reverse.CheckedChanged += new System.EventHandler(this.materialCheckBox1_CheckedChanged);
            // 
            // field_ignored
            // 
            this.field_ignored.Depth = 0;
            this.field_ignored.Hint = "";
            this.field_ignored.Location = new System.Drawing.Point(253, 13);
            this.field_ignored.MaxLength = 32767;
            this.field_ignored.MouseState = MaterialSkin.MouseState.HOVER;
            this.field_ignored.Name = "field_ignored";
            this.field_ignored.PasswordChar = '\0';
            this.field_ignored.SelectedText = "";
            this.field_ignored.SelectionLength = 0;
            this.field_ignored.SelectionStart = 0;
            this.field_ignored.Size = new System.Drawing.Size(129, 23);
            this.field_ignored.TabIndex = 27;
            this.field_ignored.TabStop = false;
            this.field_ignored.Text = "Enter Name";
            this.field_ignored.UseSystemPasswordChar = false;
            // 
            // btn_add
            // 
            this.btn_add.AutoSize = true;
            this.btn_add.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_add.Depth = 0;
            this.btn_add.Icon = null;
            this.btn_add.Location = new System.Drawing.Point(253, 51);
            this.btn_add.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_add.Name = "btn_add";
            this.btn_add.Primary = true;
            this.btn_add.Size = new System.Drawing.Size(48, 36);
            this.btn_add.TabIndex = 26;
            this.btn_add.Text = "Add";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // btn_remove
            // 
            this.btn_remove.AutoSize = true;
            this.btn_remove.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_remove.Depth = 0;
            this.btn_remove.Icon = null;
            this.btn_remove.Location = new System.Drawing.Point(307, 51);
            this.btn_remove.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_remove.Name = "btn_remove";
            this.btn_remove.Primary = true;
            this.btn_remove.Size = new System.Drawing.Size(75, 36);
            this.btn_remove.TabIndex = 25;
            this.btn_remove.Text = "Remove";
            this.btn_remove.UseVisualStyleBackColor = true;
            this.btn_remove.Click += new System.EventHandler(this.btn_remove_Click);
            // 
            // list_ignored
            // 
            this.list_ignored.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.list_ignored.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Player});
            this.list_ignored.Depth = 0;
            this.list_ignored.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.list_ignored.FullRowSelect = true;
            this.list_ignored.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.list_ignored.Location = new System.Drawing.Point(0, 0);
            this.list_ignored.MouseLocation = new System.Drawing.Point(-1, -1);
            this.list_ignored.MouseState = MaterialSkin.MouseState.OUT;
            this.list_ignored.Name = "list_ignored";
            this.list_ignored.OwnerDraw = true;
            this.list_ignored.Size = new System.Drawing.Size(221, 232);
            this.list_ignored.TabIndex = 24;
            this.list_ignored.UseCompatibleStateImageBehavior = false;
            this.list_ignored.View = System.Windows.Forms.View.Details;
            // 
            // Player
            // 
            this.Player.Text = "Player";
            this.Player.Width = 209;
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.BaseTabControl = this.materialTabControl1;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Location = new System.Drawing.Point(0, 64);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(419, 44);
            this.materialTabSelector1.TabIndex = 20;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 368);
            this.Controls.Add(this.materialTabSelector1);
            this.Controls.Add(this.materialTabControl1);
            this.Name = "Main";
            this.Text = "RequestifyTF2";
            this.Load += new System.EventHandler(this.Main_Load);
            this.materialTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        private void List_plugins_DoubleClick(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion
        private MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private MaterialSkin.Controls.MaterialCheckBox chkbox_onlywithcode;
        private MaterialSkin.Controls.MaterialLabel lbl_code;
        private MaterialSkin.Controls.MaterialLabel txtbx_GamePath;
        private MaterialSkin.Controls.MaterialRaisedButton btn_start;
        private MaterialSkin.Controls.MaterialRaisedButton btn_SelectGamePath;
        private MaterialSkin.Controls.MaterialRaisedButton btn_consoleshow;
        private MaterialSkin.Controls.MaterialListView list_plugins;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private MaterialSkin.Controls.MaterialListView list_ignored;
        private System.Windows.Forms.ColumnHeader Player;
        private MaterialSkin.Controls.MaterialRaisedButton btn_add;
        private MaterialSkin.Controls.MaterialRaisedButton btn_remove;
        private MaterialSkin.Controls.MaterialSingleLineTextField field_ignored;
        private MaterialSkin.Controls.MaterialCheckBox chkbox_reverse;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialDivider materialDivider1;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialLabel materialLabel4;
        private MaterialSkin.Controls.MaterialLabel materialLabel5;
        private MaterialSkin.Controls.MaterialLabel materialLabel6;
        private MaterialSkin.Controls.MaterialSingleLineTextField materialSingleLineTextField1;
        private MaterialSkin.Controls.MaterialRaisedButton materialRaisedButton1;
        private MaterialSkin.Controls.MaterialCheckBox materialCheckBox1;
    }
}

