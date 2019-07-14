using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Ookii.Dialogs;
using RequestifyTF2.API;
using RequestifyTF2.Utils;

namespace RequestifyTF2GUI.Controls
{
    /// <summary>
    /// Логика взаимодействия для SettingsTab.xaml
    /// </summary>
    public partial class SettingsTab : UserControl
    {
        public static SettingsTab instance;
        public SettingsTab()
        {
            InitializeComponent();
            instance = this;
        }
        private void GameBrowser_click(object sender, RoutedEventArgs e)
        {
            var a = new Games { DataContext = this };
            a.Show();
        }
        public int BackGroundMusicLenght
        {
            get { return AppConfig.CurrentConfig.MaximumBackgroundInMin; }
            set
            {
                AppConfig.CurrentConfig.MaximumBackgroundInMin = value;
                txtNum.Text = value.ToString();
                AppConfig.Save();
            }
        } 
        public int AntiSpamThredshold
        {
            get { return AppConfig.CurrentConfig.AntiSpamThredshold; }
            set
            {
                AppConfig.CurrentConfig.AntiSpamThredshold = value;
                txtNum2.Text = value.ToString();
                AppConfig.Save();
            }
        }
     
            private void cmdUp2_Click(object sender, RoutedEventArgs e)
        {
            AntiSpamThredshold++;
        }

        private void cmdDown2_Click(object sender, RoutedEventArgs e)
        {
            if(AntiSpamThredshold>0)
            { 
             AntiSpamThredshold--;
            }
        }
        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            BackGroundMusicLenght++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if(BackGroundMusicLenght>0)
            { 
             BackGroundMusicLenght--;
            }
        }
       
         private void musiclenght_changed(object sender, TextChangedEventArgs e)
         {
            if (txtNum == null)
            {
                return;
            }
            var _numValue = 0;

            if (!int.TryParse(txtNum.Text, out _numValue))
            { 
                 if(_numValue<0)
                {
                    _numValue = 0;
                }
                AppConfig.CurrentConfig.MaximumBackgroundInMin = _numValue;
                AppConfig.Save();
                txtNum.Text = _numValue.ToString();
            } 
         }
         private void antispamthreshold_changed(object sender, TextChangedEventArgs e)
        {
            if (txtNum2 == null)
            {
                return;
            }

            int _numValue;
            if (!int.TryParse(txtNum2.Text, out _numValue))
            { 
                if(_numValue<0)
                {
                    _numValue = 0;
                }
                AppConfig.CurrentConfig.AntiSpamThredshold = _numValue;
                AppConfig.Save();
                txtNum2.Text = _numValue.ToString();
        } 
            }
        private void Sample1_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;
            if (!string.IsNullOrWhiteSpace(FruitTextBox.Text))
                if (!IgnoreList.Items.Contains(FruitTextBox.Text))
                {
                    IgnoreList.Items.Add(FruitTextBox.Text);
                    if (!Instance.Config.Ignored.Contains(FruitTextBox.Text))
                    {
                        Instance.Config.Ignored.Add(FruitTextBox.Text);
                    }
                }

        }


        private void RemoveCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            Instance.Config.IgnoredReversed = true;
        }

        private void RemoveCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            Instance.Config.IgnoredReversed = false;
        }

        private void IgnoreList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IgnoreList.SelectedItem != null)
            {
                if (IgnoreList.Items.Contains(IgnoreList.SelectedItem))
                {
                    if (Instance.Config.Ignored.Contains(IgnoreList.SelectedItem))
                    {
                        Instance.Config.Ignored.Remove(IgnoreList.SelectedItem.ToString());
                    }

                    IgnoreList.Items.Remove(IgnoreList.SelectedItem);
                    return;
                }
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var s = new VistaFolderBrowserDialog())
            {
                s.UseDescriptionForTitle = true;
                s.Description = Application.Current.FindResource("cs_Select_Game_Path").ToString();

                if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (s.SelectedPath == string.Empty)
                    {
                        return;
                    }

                    var path = Patcher.ResolveFolder(s.SelectedPath);
                    if (path != "")
                    {
                        GamePath.Text = Application.Current.FindResource("cs_Current_Game_Path").ToString() + path;
                        AppConfig.CurrentConfig.GameDirectory = path;
                        AppConfig.Save();
                       
                       
                    }
                    else
                    {
                        MessageBox.Show(Application.Current.FindResource("cs_Not_Source_Engine_Game").ToString(), Application.Current.FindResource("cs_Error").ToString(),
                            MessageBoxButton.OK);
                    }
                }
            }
        }

        private void AdminBox_TextChanged(object sender, RoutedEventArgs e)
        {
            AppConfig.CurrentConfig.Admin =AdminBox.Text;
            Instance.Config.Admin = AdminBox.Text;
            AppConfig.Save();
        }
    }
}
