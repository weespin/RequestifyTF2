﻿using System;
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
using Ookii.Dialogs;
using RequestifyTF2.Utils;

namespace RequestifyTF2GUIRedone.Controls
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
    }
}
