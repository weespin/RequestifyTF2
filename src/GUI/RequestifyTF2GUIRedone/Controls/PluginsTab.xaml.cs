using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;
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
            this.DataContext = new PluginsViewModel();
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
                //if (a.Color == null)
                //{
                //    a.Color = (Brush)new BrushConverter().ConvertFrom("#87b91d47");
                //    Instance.Commands.DisableCommand(Instance.Commands.GetCommand(a.Command.Name));
                //}
                //else
                //{
                //    Instance.Commands.EnableCommand(Instance.Commands.GetCommand(a.Command.Name));
                //    a.Color = null;
                //}

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
                //if (a.Color == null)
                //{
                //    a.Color = (Brush)new BrushConverter().ConvertFrom("#87b91d47");
                //    Instance.Plugins.DisablePlugin(Instance.Plugins.GetPlugin(a.PluginName));
                //}
                //else
                //{
                //    Instance.Plugins.EnablePlugin(Instance.Plugins.GetPlugin(a.PluginName));
                //    a.Color = null;
                //}

                PluginsList.Items.Insert(index, a);
            }
        }

    }

    public class PluginsViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<PluginsAndCommandsViewModel> _plugins;
        private readonly ObservableCollection<PluginsAndCommandsViewModel> _commands;

        
        public PluginsViewModel()
        {
            _plugins = new ObservableCollection<PluginsAndCommandsViewModel>();
            Events.PluginLoaded.OnPluginLoaded += PluginLoaded_OnPluginLoaded;    
            //Plugins = CreateData();
            //Commands = CreateData();

        }

        private void PluginLoaded_OnPluginLoaded(Events.PluginLoadedArgs e)
        {
            dispatcher.Invoke(() => Plugins.Add(new PluginsAndCommandsViewModel(){Name = e.Plugin.Name}));
            Plugins.Add(new PluginsAndCommandsViewModel(){Name = e.Plugin.Name});
        }

        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        public ObservableCollection<PluginsAndCommandsViewModel> Plugins => _plugins;
        public ObservableCollection<PluginsAndCommandsViewModel> Commands { get; set; }

     

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
    public class PluginsAndCommandsViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;
        private string _name;
        private string _description;
    

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
