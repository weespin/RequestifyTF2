namespace RequestifyTF2Forms
{
    partial class Console
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
            this.txt_console = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt_console
            // 
            this.txt_console.BackColor = System.Drawing.SystemColors.InfoText;
            this.txt_console.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txt_console.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.txt_console.Location = new System.Drawing.Point(-1, 64);
            this.txt_console.Multiline = true;
            this.txt_console.Name = "txt_console";
            this.txt_console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_console.Size = new System.Drawing.Size(351, 160);
            this.txt_console.TabIndex = 3;
            this.txt_console.TextChanged += new System.EventHandler(this.txt_console_TextChanged);
            // 
            // Console
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 225);
            this.ControlBox = false;
            this.Controls.Add(this.txt_console);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Console";
            this.Text = "Console";
            this.Load += new System.EventHandler(this.Thanks_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txt_console;
    }
}