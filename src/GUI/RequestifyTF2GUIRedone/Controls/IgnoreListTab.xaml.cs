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
        private void IgnoreListButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            if (!IgnoreList.Items.Contains(IgnoreBox.Text))
            {
                IgnoreList.Items.Add(IgnoreBox.Text);
                if (!Instance.Config.Ignored.Contains(IgnoreBox.Text))
                {
                    Instance.Config.Ignored.Add(IgnoreBox.Text);
                }
            }
        }
       
        private void IgnoreListButtonRemove_OnClick(object sender, RoutedEventArgs e)
        {
            if (IgnoreList.Items.Contains(IgnoreBox.Text))
            {
                if (Instance.Config.Ignored.Contains(IgnoreBox.Text))
                {
                    Instance.Config.Ignored.Remove(IgnoreBox.Text);
                }

                IgnoreList.Items.Remove(IgnoreBox.Text);
                return;
            }

            if (IgnoreList.SelectedItem != null)
            {
                if (Instance.Config.Ignored.Contains(IgnoreList.SelectedItem))
                {
                    Instance.Config.Ignored.Remove(IgnoreList.SelectedItem.ToString());
                }

                IgnoreList.Items.Remove(IgnoreList.SelectedItem);
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
    }
}
