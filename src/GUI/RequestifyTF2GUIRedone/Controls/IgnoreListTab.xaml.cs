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
using MaterialDesignThemes.Wpf;
using RequestifyTF2.API;

namespace RequestifyTF2GUIRedone.Controls
{
    /// <summary>
    /// Логика взаимодействия для IgnoreListTab.xaml
    /// </summary>
    public partial class IgnoreListTab : UserControl
    {
        public static  IgnoreListTab instance;
        public IgnoreListTab()
        {
            InitializeComponent();
            instance = this;
        }
        private void Sample1_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;
            if (!string.IsNullOrWhiteSpace(FruitTextBox.Text))
                if (!FruitListBox.Items.Contains(FruitTextBox.Text))
                {
                    FruitListBox.Items.Add(FruitTextBox.Text);
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

        private void FruitListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(FruitListBox.SelectedItem!=null)
            {
                if (FruitListBox.Items.Contains(FruitListBox.SelectedItem))
                {
                    if (Instance.Config.Ignored.Contains(FruitListBox.SelectedItem))
                    {
                        Instance.Config.Ignored.Remove(FruitListBox.SelectedItem.ToString());
                    }

                    FruitListBox.Items.Remove(FruitListBox.SelectedItem);
                    return;
                }
            }
        }
    }
}
