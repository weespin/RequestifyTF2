using System.Windows.Controls;

namespace RequestifyTF2GUI.Controls
{
    /// <summary>
    /// Логика взаимодействия для StatisticsTab.xaml
    /// </summary>
    public partial class StatisticsTab : UserControl
    {
        public static StatisticsTab instance;
        public StatisticsTab()
        {
            InitializeComponent();
            instance = this;
        }
    }
 
}
