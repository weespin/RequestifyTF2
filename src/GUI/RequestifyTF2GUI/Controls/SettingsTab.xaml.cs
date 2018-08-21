// RequestifyTF2GUI
// Copyright (C) 2018  Villiam Nmerukini
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Ookii.Dialogs;
using RequestifyTF2.API;
using RequestifyTF2.Utils;
using RequestifyTF2.API.IgnoreList;
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
        private void Sample1_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;
            if (!string.IsNullOrWhiteSpace(FruitTextBox.Text))
                if (!IgnoreList.Items.Contains(FruitTextBox.Text))
                {
                    IgnoreList.Items.Add(FruitTextBox.Text);
                    if (!RequestifyTF2.API.IgnoreList.IgnoreList.Contains(FruitTextBox.Text))
                    {
                        RequestifyTF2.API.IgnoreList.IgnoreList.Add(FruitTextBox.Text);
                    }
                }

        }


        private void RemoveCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            RequestifyTF2.API.IgnoreList.IgnoreList.Reversed = true;
        }

        private void RemoveCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            RequestifyTF2.API.IgnoreList.IgnoreList.Reversed = false;
        }

        private void IgnoreList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IgnoreList.SelectedItem != null)
            {
                if (IgnoreList.Items.Contains(IgnoreList.SelectedItem))
                {
                    if (RequestifyTF2.API.IgnoreList.IgnoreList.Contains(IgnoreList.SelectedItem.ToString()))
                    {
                        RequestifyTF2.API.IgnoreList.IgnoreList.Remove(IgnoreList.SelectedItem.ToString());
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
    }
}
