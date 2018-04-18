namespace RequestifyTF2.Api
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;

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
                    if (!Instance.isMuted)
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
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
    }

    internal static class AutoexecChecker
    {
        public static void Check()
        {
            if (Instance.Config.GameDir == string.Empty || !Directory.Exists(Instance.Config.GameDir))
            {
                Console.WriteLine("Please set game directory");
                return;
            }

            if (File.Exists(Instance.Config.GameDir + "/cfg/autoexec.cfg"))
            {
                var c = false;
                var p = false;
                var lines = File.ReadAllLines(Instance.Config.GameDir + "/cfg/autoexec.cfg");
                foreach (var line in lines)
                {
                    if (line.Contains("con_logfile \"console.log\""))
                    {
                        c = true;
                        break;
                    }
                }

                if (!c)
                {
                    File.AppendAllText(
                        Instance.Config.GameDir + "/cfg/autoexec.cfg",
                        Environment.NewLine + "con_logfile \"console.log\"");
                }

                foreach (var line in lines)
                    if (line.Contains("bind KP_PGUP \"exec requestify\""))
                    {
                        p = true;
                        break;
                    }

                if (!p)
                {
                    File.AppendAllText(
                        Instance.Config.GameDir + "/cfg/autoexec.cfg",
                        Environment.NewLine + "bind KP_PGUP \"exec requestify\"");
                }
            }
            else
            {
                File.AppendAllText(
                    Instance.Config.GameDir + "/cfg/autoexec.cfg",
                    "con_logfile \"console.log\"" + Environment.NewLine + "bind F11 \"exec requestify\"");
            }
        }
    }
}