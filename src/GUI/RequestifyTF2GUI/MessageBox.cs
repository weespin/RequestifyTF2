
namespace RequestifyTF2Forms
{
    using System;
    using System.Drawing;

    using MaterialSkin.Controls;

    using RequestifyTF2GUI.Properties;

    public partial class MessageBox : MaterialForm
    {
        public MessageBox()
        {
            this.InitializeComponent();
            this.lbl_text.Font = this.SkinManager.ROBOTO_REGULAR_11;
            this.Icon = Resources.Icon;
        }

        public string Color
        {
            set
            {
                // ReSharper disable once ArrangeThisQualifier
                // ReSharper disable once ArrangeAccessorOwnerBody
                this.lbl_text.ForeColor = ColorTranslator.FromHtml(value);
            }
        }

        public string MessageText
        {
            set
            {
                // ReSharper disable once ArrangeThisQualifier
                // ReSharper disable once ArrangeAccessorOwnerBody
                this.lbl_text.Text = value;
            }
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}