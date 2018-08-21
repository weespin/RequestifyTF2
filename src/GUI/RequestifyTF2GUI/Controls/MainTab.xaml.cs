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
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using RequestifyTF2;
using RequestifyTF2.API;
using RequestifyTF2.API.Events;

namespace RequestifyTF2GUI.Controls
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public static class AssemblyExtension
    {
        public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            var buffer = new byte[2048];
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);
            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secoundsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var linktimeutc = epoch.AddSeconds(secoundsSince1970);
            var tz = target ?? TimeZoneInfo.Local;
            return TimeZoneInfo.ConvertTimeFromUtc(linktimeutc, tz);

        }
    }
    public partial class Main : UserControl
    {
        public string Version
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

       
        public static Main instance;
        public Main()
        {
           
            InitializeComponent();
            instance = this;
#if DEBUG
            version.Text =  "DEV: Built at "+System.Reflection.Assembly.GetExecutingAssembly().GetLinkerTime();
#else
            version.Text = Version;
#endif
        }
        private void AdminBoxFocusLost(object sender, RoutedEventArgs e)
        {
            //AppConfig.CurrentConfig.Admin = Main.instance.AdminBox.Text;
            AppConfig.Save();
        }
        private void MutedCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            Requestify.IsMuted = true;
        }

        private void MutedCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            Requestify.IsMuted = false;
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (Requestify.GameDir == string.Empty)
            {
                MessageBox.Show(
                    Application.Current.FindResource("cs_Set_Game_Dir").ToString(),
                    Application.Current.FindResource("cs_Error").ToString());

                return;
            }
            MainWindow.instance._started = Runner.Start();
            if (MainWindow.instance._started)
            {
                //Its easy to catch events :)

                Events.OnUndefinedMessage += MainWindow.instance.UndefinedMessage_OnUndefinedMessage;
                Main.instance.StartButton.Content = Application.Current.FindResource("cs_Stop").ToString();
                Main.instance.StatusLabel.Text = Application.Current.FindResource("cs_Status_Working").ToString();
            }
        }

    }

}
