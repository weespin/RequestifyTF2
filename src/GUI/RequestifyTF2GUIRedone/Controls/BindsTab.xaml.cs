using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace RequestifyTF2GUIRedone.Controls
{
    /// <summary>
    /// Логика взаимодействия для BindsTab.xaml
    /// </summary>
    public class BindsViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;

        private string _link;
        private string _numpadkey;
        private string _bindType;
        private int _id;



        public int Id
        
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }
        

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
                if (_bindType == "Stop")
                {
                    Link = "";
                }

                if (_bindType == "YoutubeMusic")
                {
                    if (Link != "")
                    {
                        if (!Regexes.IsYoutubeVideo(Link))
                        {
                            Link = "";
                        }
                    }
                }

                if (_bindType == "LocalMusic")
                {
                    if (Link != "")
                    {
                        if (Regexes.IsYoutubeVideo(Link))
                        {
                            Link = "";
                        }
                    }
                }
                
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (NumpadKey != null && BindType != null && Link != null && AppConfig.CurrentConfig.Buttons!=null)
            {
               
                    AppConfig.CurrentConfig.Buttons.buttons[Id].BindType = BindType;
                    AppConfig.CurrentConfig.Buttons.buttons[Id].Link = Link;
                    AppConfig.Save();
                
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class Regexes
    {
    static    Regex youtube = new Regex(@"youtube\..+?/watch.*?v=(.*?)(?:&|/|$)");
        static Regex shortregex = new Regex(@"youtu\.be/(.*?)(?:\?|&|/|$)");

        public static bool IsYoutubeVideo(string link)
        {
            return false || youtube.Match(link.ToString()).Success || shortregex.Match(link.ToString()).Success;
        }
    }

    public class ListsAndGridsViewModel : INotifyPropertyChanged
    {
    
        public ListsAndGridsViewModel ()
        {

            AppConfig.Load();
            BindItems = new System.Collections.ObjectModel.ObservableCollection<BindsViewModel>(AppConfig.CurrentConfig.Buttons.buttons);
           
        }

      

        public ObservableCollection<BindsViewModel> BindItems { get; set; }

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
            var cell = (BindsViewModel)((DataGridCell)sender).DataContext;

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

            if (cell.BindType == "YoutubeMusic")
            {

                id = cell.Id;
                ExecuteRunDialog(sender);
            }
        }

        public static int id;
        private async void ExecuteRunDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new YoutubeDialog
            {
                DataContext = new SampleDialogViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

        
        }
        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            var vid = (DialogHost) sender;
            var con = (YoutubeDialog) vid.DialogContent;
            var link = (SampleDialogViewModel)con.DataContext;

            AppConfig.CurrentConfig.Buttons.buttons[id].Link = link.Link;
        }


    }
    public class SampleDialogViewModel : INotifyPropertyChanged
    {
        private string _link;

        public string Link
        {
            get { return _link; }
            set
            {
                this.MutateVerbose(ref _link, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
    public static class NotifyPropertyChangedExtension
    {
        public static void MutateVerbose<TField>(this INotifyPropertyChanged instance, ref TField field, TField newValue, Action<PropertyChangedEventArgs> raise, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TField>.Default.Equals(field, newValue)) return;
            field = newValue;
            raise?.Invoke(new PropertyChangedEventArgs(propertyName));
        }
    }
}
