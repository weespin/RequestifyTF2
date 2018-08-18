using System;
using System.Windows;
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
            RequestifyTF2.API.Requestify._writer.WriteLineEvent += _writer_WriteLineEvent;
            RequestifyTF2.API.Requestify._writer.WriteEvent += _writer_WriteEvent;
            instance = this;
            InitializeComponent();
           
        }

        private void _writer_WriteEvent(object sender, RequestifyTF2.Utils.RequestifyConsoleHookArgs e)
        {
            Console.Text += e.Value;
        }

        private void _writer_WriteLineEvent(object sender, RequestifyTF2.Utils.RequestifyConsoleHookArgs e)
        {
            Console.Text += e.Value+Environment.NewLine;
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (debugchk.IsChecked != null) RequestifyTF2.API.Requestify.Debug = debugchk.IsChecked.Value;
        }

        private void Debugchk_OnUnchecked(object sender, RoutedEventArgs e)
        {
            if (debugchk.IsChecked != null) RequestifyTF2.API.Requestify.Debug = debugchk.IsChecked.Value;
        }
    }
}
