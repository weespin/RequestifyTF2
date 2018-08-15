using System.Windows.Controls;

namespace RequestifyTF2GUI.Controls
{
    /// <summary>
    /// Логика взаимодействия для ConsoleTab.xaml
    /// </summary>
    public partial class ConsoleTab : UserControl
    {
        public static ConsoleTab instance;
        public ConsoleTab()
        {
            InitializeComponent();
            instance = this;
        }
       
    }
}
