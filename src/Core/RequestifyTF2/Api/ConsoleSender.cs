using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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
            var text = "";
            switch (cmd)
            {
                case Command.Chat:
                    text = "say " + cmnd;
                    break;
                case Command.Echo:
                    text = "echo " + cmnd;
                    break;
                case Command.Raw:
                    text = cmnd;
                    break;
            }
            File.WriteAllText(Instance.Config.GameDir + "/cfg/requestify.cfg", text);
            if (Instance.Config.AhkPath != string.Empty)
            {
                Process.Start(Instance.Config.AhkPath);
            }
            else
            {
                Console.WriteLine("ERROR, AutoHotKey Path is not found!");
            }
        }
    }

   static class AutoexecChecker
    {
     

        public static void Check()
        {
            if (Instance.Config.GameDir == "" || !Directory.Exists(Instance.Config.GameDir))
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
                    File.AppendAllText(Instance.Config.GameDir + "/cfg/autoexec.cfg",
                        Environment.NewLine + "con_logfile \"console.log\"");
                }

                foreach (var line in lines)
                    if (line.Contains("bind F11 \"exec requestify\""))
                    {
                        p = true;
                        break;
                    }

                if (!p)
                {
                    File.AppendAllText(Instance.Config.GameDir + "/cfg/autoexec.cfg",
                        Environment.NewLine + "bind F11 \"exec requestify\"");
                }
            }
            else
            {
                File.AppendAllText(Instance.Config.GameDir + "/cfg/autoexec.cfg",
                    "con_logfile \"console.log\"" + Environment.NewLine + "bind F11 \"exec requestify\"");
            }
        }

     
    }
}