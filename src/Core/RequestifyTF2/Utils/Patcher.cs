using System;
using System.IO;
using RequestifyTF2.Api;

internal static class Patcher
{
    public static void PatchAutoExec()
    {
        if (Instance.Config.GameDir == string.Empty || !Directory.Exists(Instance.Config.GameDir))
        {
            Console.WriteLine("Please set game directory");
            return;
        }

        if (File.Exists(Instance.Config.GameDir + "/cfg/autoexec.cfg"))
        {
            var flag1 = false;
            var flag2 = false;
            var lines = File.ReadAllLines(Instance.Config.GameDir + "/cfg/autoexec.cfg");
            foreach (var line in lines)
            {
                if (line.Contains("con_logfile \"console.log\""))
                {
                    flag1 = true;
                    break;
                }
            }

            if (!flag1)
            {
                File.AppendAllText(
                    Instance.Config.GameDir + "/cfg/autoexec.cfg",
                    Environment.NewLine + "con_logfile \"console.log\"");
            }

            foreach (var line in lines)
                if (line.Contains("bind KP_PGUP \"exec requestify\""))
                {
                    flag2 = true;
                    break;
                }

            if (!flag2)
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