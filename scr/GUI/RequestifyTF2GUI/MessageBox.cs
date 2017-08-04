using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;

namespace RequestifyTF2Forms
{
    public partial class MessageBox : MaterialForm
    {
    
        public MessageBox()
        {
            InitializeComponent();
            this.materialLabel1.Font = SkinManager.ROBOTO_REGULAR_11;
        
        }

        public string MessageText
        {
            
            set { materialLabel1.Text = value; }
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

   

      
       
    }
}
