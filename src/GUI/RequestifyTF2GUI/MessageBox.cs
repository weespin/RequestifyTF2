using System;
using System.Drawing;
using MaterialSkin.Controls;
using RequestifyTF2GUI.Properties;

namespace RequestifyTF2Forms
{
    public partial class MessageBox : MaterialForm
    {
        public MessageBox()
        {
            InitializeComponent();
            lbl_text.Font = SkinManager.ROBOTO_REGULAR_11;
            Icon = Resources.Icon;
        }

        public string Color
        {
            set => lbl_text.ForeColor = ColorTranslator.FromHtml(value);
        }

        public string MessageText
        {
            set => lbl_text.Text = value;
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}