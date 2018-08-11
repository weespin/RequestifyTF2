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
using RequestifyTF2.API;

namespace RequestifyTF2GUIRedone.Controls
{
    /// <summary>
    /// Логика взаимодействия для PluginsTab.xaml
    /// </summary>
    public partial class PluginsTab : UserControl
    {
        public static PluginsTab instance;
        public PluginsTab()
        {
            InitializeComponent();
            instance = this;
        }
        private void CommandsBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var a = CommandsBox.SelectedItem as MainWindow.CommandItem;
            var index = CommandsBox.SelectedIndex;
            if (a != null)
            {
                CommandsBox.Items.RemoveAt(index);
                if (a.Color == null)
                {
                    a.Color = (Brush)new BrushConverter().ConvertFrom("#87b91d47");
                    Instance.Commands.DisableCommand(Instance.Commands.GetCommand(a.Command.Name));
                }
                else
                {
                    Instance.Commands.EnableCommand(Instance.Commands.GetCommand(a.Command.Name));
                    a.Color = null;
                }

                CommandsBox.Items.Insert(index, a);
            }
        }

        private void PluginsList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var a = PluginsList.SelectedItem as MainWindow.PluginItem;
            var index = PluginsList.SelectedIndex;
            if (a != null)
            {
                PluginsList.Items.RemoveAt(index);
                if (a.Color == null)
                {
                    a.Color = (Brush)new BrushConverter().ConvertFrom("#87b91d47");
                    Instance.Plugins.DisablePlugin(Instance.Plugins.GetPlugin(a.PluginName));
                }
                else
                {
                    Instance.Plugins.EnablePlugin(Instance.Plugins.GetPlugin(a.PluginName));
                    a.Color = null;
                }

                PluginsList.Items.Insert(index, a);
            }
        }
    }
}
