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
    }

    public class PluginsViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<PluginsAndCommandsViewModel> _plugins;
        private readonly ObservableCollection<PluginsAndCommandsViewModel> _commands;

        
        public PluginsViewModel()
        {
            _plugins = new ObservableCollection<PluginsAndCommandsViewModel>();
            _commands = new ObservableCollection<PluginsAndCommandsViewModel>();
            Events.PluginLoaded.OnPluginLoaded += PluginLoaded_OnPluginLoaded;
            Events.CommandRegistered.OnCommandRegistered += CommandRegistered_OnCommandRegistered;
            //Plugins = CreateData();
            //Commands = CreateData();

        }

        private void CommandRegistered_OnCommandRegistered(Events.CommandRegisteredArgs e)
        {
            dispatcher.Invoke(() => Commands.Add(new PluginsAndCommandsViewModel() { IsSelected = true, Type = PluginsAndCommandsViewModel.MType.Command, Name = e.Command.Name, Description = e.Command.Help }));
        }

        private void PluginLoaded_OnPluginLoaded(Events.PluginLoadedArgs e)
        {
            dispatcher.Invoke(() => Plugins.Add(new PluginsAndCommandsViewModel(){IsSelected = true,Type = PluginsAndCommandsViewModel.MType.Plugin,Name = e.Plugin.Name, Description = e.Plugin.Desc}));
           
        }

        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        public ObservableCollection<PluginsAndCommandsViewModel> Plugins => _plugins;
        public ObservableCollection<PluginsAndCommandsViewModel> Commands => _commands;

     

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
        private MType _mtype;

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

        public MType Type
        {
            get { return _mtype; }
            set
            {
                if (_mtype == value) return;
                _mtype = value;
                OnPropertyChanged();
            }
        }

        public enum MType
        {
            Plugin,Command
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                if (this.Type == MType.Plugin)
                {
                    if (!IsSelected)
                    {
                        //PLUGIN GOING TO DISABLE
                        if (Instance.Plugins.GetPlugin(this.Name) != null)
                        {
                            Instance.Plugins.DisablePlugin(Instance.Plugins.GetPlugin(this.Name));
                        }
                    }
                    else
                    {
                        if (Instance.Plugins.GetPlugin(this.Name) != null)
                        {
                            Instance.Plugins.EnablePlugin(Instance.Plugins.GetPlugin(this.Name));
                        }
                    }
                }
                else
                {
                    if (!IsSelected)
                    {
                        //PLUGIN GOING TO DISABLE
                        if (Instance.Commands.GetCommand(this.Name) != null)
                        {
                            Instance.Commands.DisableCommand(Instance.Commands.GetCommand(this.Name));
                        }
                    }
                    else
                    {
                        if (Instance.Commands.GetCommand(this.Name) != null)
                        {
                            Instance.Commands.EnableCommand(Instance.Commands.GetCommand(this.Name));
                        }
                    }
                }
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
