// RequestifyTF2
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
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace RequestifyTF2.API.ConsoleAPI
{
    public static class ConsoleSender
    {
        public enum Command
        {
            Chat,
            Echo,
            Raw
        }

        public static void SendCommand(string cmnd, Command cmd)
        {
            var text = string.Empty;
            switch (cmd)
            {
                case Command.Chat:
                    if (!Requestify.IsMuted)
                    {
                        text = "say " + cmnd;
                    }

                    break;
                case Command.Echo:
                    text = "echo " + cmnd;
                    break;
                case Command.Raw:
                    text = cmnd;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            File.WriteAllText(Requestify.GameDir + "/cfg/requestify.cfg", text);
            Task.Run(
                () =>
                {
                    keybd_event(0x2E, 0x53, 0, 0);
                    Thread.Sleep(30);
                    keybd_event(0x2E, 0x53, 0x2, 0);
                });
            Thread.Sleep(100);
            File.WriteAllText(Requestify.GameDir + "/cfg/requestify.cfg", string.Empty);
            Logger.Nlogger.Debug($"Sent {text} using ConsoleSender");
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
    }
}