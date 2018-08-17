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
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
    }
}