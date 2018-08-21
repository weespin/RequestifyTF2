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
