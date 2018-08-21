// RequestifyTF2GUIOld(unsupported)
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

using System.Media;
using RequestifyTF2Forms;

namespace RequestifyTF2GUI.MessageBox
{
    internal class MessageBox
    {
        public void Show(string message, string title, Sounds sound = Sounds.None)
        {
            var msgbox = new RequestifyTF2Forms.MessageBox {MessageText = message, Text = title, Color = "#F44336"};
            msgbox.ShowDialog(Main.instance);
            msgbox.BringToFront();
            msgbox.Activate();
            msgbox.Focus();
            switch (sound)
            {
                case Sounds.None:
                    break;
                case Sounds.Asterik:
                    SystemSounds.Asterisk.Play();
                    break;
                case Sounds.Beep:
                    SystemSounds.Beep.Play();
                    break;
                case Sounds.Exclamation:
                    SystemSounds.Exclamation.Play();
                    break;
                case Sounds.Hand:
                    SystemSounds.Hand.Play();
                    break;
                case Sounds.Question:
                    SystemSounds.Question.Play();
                    break;
                default:
                    SystemSounds.Beep.Play();
                    break;
            }
        }

        internal enum Sounds
        {
            Asterik,

            Beep,

            Exclamation,

            Hand,

            Question,

            None
        }
    }
}