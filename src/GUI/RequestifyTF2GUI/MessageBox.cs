using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using RequestifyTF2GUI.Properties;

namespace RequestifyTF2Forms
{
    public partial class MessageBox : MaterialForm
    {
    
        public MessageBox()
        {
            InitializeComponent();
            this.lbl_text.Font = SkinManager.ROBOTO_REGULAR_11;
             Icon = Resources.Icon;
            

        }

        public string Color
        {
            set { lbl_text.ForeColor = System.Drawing.ColorTranslator.FromHtml(value); }
        }
        public string MessageText
        {
            
            set { lbl_text.Text = value; }
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
    }
}
