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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Threading;
using RequestifyTF2.API;
using RequestifyTF2.API.Events;
using RequestifyTF2.Managers;

namespace RequestifyTF2GUI.Controls
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
            Events.OnPluginLoaded += PluginLoaded_OnPluginLoaded;
            Events.OnCommandRegistered += CommandRegistered_OnCommandRegistered;
        
            //Plugins = CreateData();
            //Commands = CreateData();

        }

    
        private void CommandRegistered_OnCommandRegistered(RequestifyEventArgs.CommandRegisteredArgs e)
        {
            dispatcher.Invoke(() => Commands.Add(new PluginsAndCommandsViewModel() { IsSelected = true, Type = PluginsAndCommandsViewModel.MType.Command, Name = e.Command.Name, Description = e.Command.Help }));
        }

        private void PluginLoaded_OnPluginLoaded(RequestifyEventArgs.PluginLoadedArgs e)
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
                        if (PluginManager.GetPlugin(this.Name) != null)
                        {
                           PluginManager.GetPlugin(this.Name).Disable();
                        }
                    }
                    else
                    {
                        if (PluginManager.GetPlugin(this.Name) != null)
                        {
                            PluginManager.GetPlugin(this.Name).Enable();
                        }
                    }
                }
                else
                {
                    if (!IsSelected)
                    {
                        //PLUGIN GOING TO DISABLE
                        if (CommandManager.GetCommand(this.Name) != null)
                        {
                          CommandManager.GetCommand(this.Name).Disable();
                        }
                    }
                    else
                    {
                        if (CommandManager.GetCommand(this.Name) != null)
                        {
                          CommandManager.GetCommand(this.Name).Enable();
                        }
                    }
                }
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
