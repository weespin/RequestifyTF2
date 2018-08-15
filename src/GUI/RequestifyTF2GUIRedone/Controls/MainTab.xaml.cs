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
using RequestifyTF2;
using RequestifyTF2.API;

namespace RequestifyTF2GUIRedone.Controls
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        public static Main instance;
        public Main()
        {
           
            InitializeComponent();
            instance = this;
        }
        private void AdminBoxFocusLost(object sender, RoutedEventArgs e)
        {
            //AppConfig.CurrentConfig.Admin = Main.instance.AdminBox.Text;
            AppConfig.Save();
        }
        private void MutedCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            Instance.IsMuted = true;
        }

        private void MutedCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            Instance.IsMuted = false;
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (Instance.Config.GameDir == string.Empty)
            {
                MessageBox.Show(
                    Application.Current.FindResource("cs_Set_Game_Dir").ToString(),
                    Application.Current.FindResource("cs_Error").ToString());

                return;
            }
            MainWindow.instance._started = Runner.Start();
            if (MainWindow.instance._started)
            {
                //Its easy to catch events :)

                RequestifyTF2.API.Events.UndefinedMessage.OnUndefinedMessage += MainWindow.instance.UndefinedMessage_OnUndefinedMessage;
                Main.instance.StartButton.Content = Application.Current.FindResource("cs_Stop").ToString();
                Main.instance.StatusLabel.Text = Application.Current.FindResource("cs_Status_Working").ToString();
            }
        }

    }

}
