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
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace RequestifyTF2GUIRedone.Controls
{
    /// <summary>
    /// Логика взаимодействия для BindsTab.xaml
    /// </summary>
    public class SelectableViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;

        private string _link;
        private string _numpadkey;
        private string _bindType;

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
        public string NumpadKey
        {
            get { return _numpadkey; }
            set
            {
                if (_numpadkey == value) return;
                _numpadkey = value;
                OnPropertyChanged();
            }
        }
        public string Link
        {
            get { return _link; }
            set
            {
                if (_link == value) return;
                _link = value;
                OnPropertyChanged();
            }
        }
        public string BindType
        {
            get { return _bindType; }
            set
            {
                if (_bindType == value) return;
                _bindType = value;
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
    public class ListsAndGridsViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<SelectableViewModel> _items1;
        private bool? _isAllItems3Selected;

        public ListsAndGridsViewModel()
        {
            _items1 = CreateData();
          
        }

        private static ObservableCollection<SelectableViewModel> CreateData()
        {
            var a = new List<SelectableViewModel>();
            for (int i = 0; i < 10; i++)
            {
                a.Add(new SelectableViewModel()
                {
                   NumpadKey = "NUMPAD "+ i,
                    BindType = "Stop",
                    IsSelected = true,
                    Link = ""
                });
            }
            return new ObservableCollection<SelectableViewModel>(a);

        }
        public ObservableCollection<SelectableViewModel> BindItems => _items1;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<string> BindType => Enum.GetNames(typeof(BindsTab.BindType));
    }
    public partial class BindsTab : UserControl
    {
        public static BindsTab instance;
        public enum BindType{
            Stop,
            LocalMusic,
            YoutubeMusic
        }

        public BindsTab()
        {
            InitializeComponent();
            instance = this;
            this.DataContext = new ListsAndGridsViewModel();
           
        }
      

       
        private void CellDoubleClick(object sender, RoutedEventArgs e)
        {
            var cell = (SelectableViewModel)((DataGridCell)sender).DataContext;

            if (cell.BindType == "Stop")
            {
                return;
            }

            if (cell.BindType == "LocalMusic")
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "MP3 File (*.mp3)|*.mp3";

                var s = dialog.ShowDialog();
                if (s == true)
                {
                    var file = dialog.FileName;
                    cell.Link = file;
                }
            }
        }
      
    }
}
