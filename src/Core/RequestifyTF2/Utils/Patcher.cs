using System;
using System.IO;
using System.Linq;
using RequestifyTF2.API;

namespace RequestifyTF2.Utils
{
    public static class Patcher
    {
        public static string ResolveFolder(string a)
        {
            var dirs = Directory.GetDirectories(a);

            if (dirs.Any(n => n.Contains("cfg")))
            {
                return a;
            }

            foreach (var dir in dirs)
            {
                var cdir = Directory.GetDirectories(dir);
                var bin = false;
                var cfg = false;
                foreach (var dirz in cdir)
                {
                    var pal = dirz;
                    var z = pal.Remove(0, dir.Length);

                    if (z.Contains("cfg"))
                    {
                        cfg = true;
                    }

                    if (z.Contains("bin"))
                    {
                        bin = true;
                    }

                    if (bin && cfg)
                    {
                        return dir;
                    }
                }
            }

            return "";
        }

        public static void PatchAutoExec()
        {
            if (Instance.Config.GameDir == string.Empty || !Directory.Exists(Instance.Config.GameDir))
            {
                Console.WriteLine(Localization.Localization.CORE_SET_DIRECTORY);
                return;
            }
            var cfgpath = Instance.Config.GameDir + "/cfg/autoexec.cfg";
            WriteToCfg(cfgpath, "con_logfile \"console.log\"");
            WriteToCfg(cfgpath, "bind kp_del \"exec requestify\"");
            WriteToCfg(cfgpath, "bind kp_end \"echo NUMPAD1\"");
            WriteToCfg(cfgpath, "bind kp_downarrow \"echo NUMPAD2\"");
            WriteToCfg(cfgpath, "bind kp_pgdn \"echo NUMPAD3\"");
            WriteToCfg(cfgpath, "bind kp_leftarrow \"echo NUMPAD4\"");
            WriteToCfg(cfgpath, "bind kp_5 \"echo NUMPAD5\"");
            WriteToCfg(cfgpath, "bind kp_rightarrow \"echo NUMPAD6\"");
            WriteToCfg(cfgpath, "bind kp_home \"echo NUMPAD7\"");
            WriteToCfg(cfgpath, "bind kp_uparrow \"echo NUMPAD8\"");
            WriteToCfg(cfgpath, "bind kp_pgup \"echo NUMPAD9\"");
            WriteToCfg(cfgpath, "bind kp_ins \"echo NUMPAD0\"");
        }
        private static void WriteToCfg(string cfgfile ,string str)
        {
            if (!File.Exists(cfgfile))
            {
                File.Create(cfgfile);
            }

            try
            {
                var lines = File.ReadAllLines(cfgfile);
                
                if (!lines.Contains(str))
                {
                    File.AppendAllText(
                        cfgfile, Environment.NewLine +
                                 str);
                }
            }
            catch (Exception e)
            {
                Logger.Write(Logger.Status.Error,e.ToString());
            }
        } 
    }
}