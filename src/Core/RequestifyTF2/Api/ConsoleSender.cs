using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace RequestifyTF2.Api
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
                    if (!Instance.isMuted) text = "say " + cmnd;
                    break;
                case Command.Echo:
                    text = "echo " + cmnd;
                    break;
                case Command.Raw:
                    text = cmnd;
                    break;
            }

            File.WriteAllText(Instance.Config.GameDir + "/cfg/requestify.cfg", text);

            Task.Run(
                () =>
                {
                    Thread.Sleep(1000);
                    keybd_event(0x69, 0x49, 0, 0);
                    Thread.Sleep(1);
                    keybd_event(0x69, 0x49, 0x2, 0);
                });
            Thread.Sleep(100);
            File.WriteAllText(Instance.Config.GameDir + "/cfg/requestify.cfg", "");
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
    }
}