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
            if (Requestify.GameDir == string.Empty || !Directory.Exists(Requestify.GameDir))
            {
                Logger.Nlogger.Error(Localization.Localization.CORE_SET_DIRECTORY);
                return;
            }
            var cfgpath = Requestify.GameDir + "/cfg/autoexec.cfg";
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
            var lines = File.ReadAllLines(cfgfile);
            if (!lines.Contains(str))
            {
                File.AppendAllText(
                cfgfile,Environment.NewLine+
                   str);
            }
        } 
    }
}