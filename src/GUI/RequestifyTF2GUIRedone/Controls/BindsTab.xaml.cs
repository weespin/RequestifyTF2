using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace RequestifyTF2GUIRedone.Controls
{
    /// <summary>
    /// Логика взаимодействия для BindsTab.xaml
    /// </summary>
    public partial class BindsTab : UserControl
    {
        public static BindsTab instance;
        public enum BindType{
            Stop,
            LocalMusic,
            YoutubeMusic,


        }
        public class Bind
        {
            public string NumpadKey { get; set; }
            public BindType BindType { get; set; }
            public string Link { get; set; }
            public bool Enabled { get; set; }
        }
        public BindsTab()
        {
            InitializeComponent();
            instance = this;
            for (int a = 0; a < 11; a++)
            {
                List.Items.Add(new Bind {NumpadKey="NUMPAD "+a,BindType= BindType.LocalMusic,Link = "" });
            }
          
        }
        private void numbutton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int num = Convert.ToInt32(button.CommandParameter.ToString().Replace("NUMPAD", ""));
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "MP3 File (*.mp3)|*.mp3";

            var s = dialog.ShowDialog();
            if (s == true)
            {
                var file = dialog.FileName;
                AppConfig.CurrentConfig.Buttons.buttons[num] = file
                    ;
                AppConfig.Save();
            }
        }
    }
}
