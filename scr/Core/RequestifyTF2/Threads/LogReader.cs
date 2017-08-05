using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using RequestifyTF2.Api;
using RequestifyTF2.Commands;

namespace RequestifyTF2
{
    internal class ReaderThread
    {
        public static void Starter()
        {
            var thread = new Thread(Read);
            thread.IsBackground = true;
            thread.Start();
        }
      
        public static void Read()
        {
            var wh = new AutoResetEvent(false);
            var fsw = new FileSystemWatcher(".")
            {
                Filter = Instance.Config.GameDir + "/console.log",
                EnableRaisingEvents = true
            };
            fsw.Changed += (s, e) => wh.Set();
            if (!File.Exists(Instance.Config.GameDir + "/console.log"))
                File.Create(Instance.Config.GameDir + "/console.log");
            var fs = new FileStream(Instance.Config.GameDir + "/console.log", FileMode.Open, FileAccess.Read,
                FileShare.ReadWrite);
            using (var sr = new StreamReader(fs, Encoding.GetEncoding("Windows-1251")))
            {
                var s = "";
                while (true)
                {
                    s = sr.ReadLine();
                    if (!string.IsNullOrEmpty(s))
                    {
                        Encoding utf8 = Encoding.GetEncoding("UTF-8");
                        Encoding win1251 = Encoding.GetEncoding("Windows-1251");

                        byte[] utf8Bytes = win1251.GetBytes(s);
                        byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
                        s=  win1251.GetString(win1251Bytes);
                        TextChecker(s);

                    }
                    else
                        wh.WaitOne(30);
                }
            }
        }
        //todo: use regex?
        public static void TextChecker(string s)
        {
            if (s.Contains(":") && s.Split(null).Length > 3)
            {
                s = s.Trim();
                var splitted = s.Split(null);
                //lets find :?
                var selector = 0;
                for (var i = 0; i < splitted.Length; i++)
                    if (splitted[i] == ":")
                        selector = i;
                var name = "";
                if (selector == 0)
                    return;
                for (var i = 0; i < selector; i++)
                    name += splitted[i];
                var arguments = new List<string>();
                if (splitted.Length > selector + 1)
                {
                    var command = splitted[selector + 1];

                    if (splitted.Length > selector + 2)
                        for (var i = selector + 2; i < splitted.Length; i++)
                            arguments.Add(splitted[i]);
                    if (command != "")
                    {
                        Executer.Execute(name, command, arguments);
                    }
                }
            }
            else if (s.Contains("killed") && s.Contains("with") && s.EndsWith(".")|| s.Contains("killed") && s.Contains("with") && s.EndsWith("(crit)"))
            {
                s = s.Trim();
                var splitted = s.Split(null);
                //its 100% kill
                //Lets find killer!
                var killerselector = 0;
                for (var i = 0; i < splitted.Length; i++)
                    if (splitted[i] == "killed")
                        killerselector = i;
                var killer = "";
                if (killerselector == 0)
                    return;
                for (var i = 0; i < killerselector; i++)
                    killer += splitted[i] + " ";
                killer = killer.Trim();
                killer = killer.TrimEnd();
                //Okay we found a killer, lets find which guy was killed?
                var deathselector = 0;
                for (var i = 0; i < splitted.Length; i++)
                    if (splitted[i] == "with")
                        deathselector = i;
                var killed = "";
                if (deathselector == 0)
                    return;
                for (var i = killerselector + 1; i < deathselector; i++)
                    killed += splitted[i] + " ";
                killed = killed.Trim();
                killed = killed.TrimEnd();
                var weapon = "";
                for (var i = deathselector + 1; i < splitted.Length; i++)
                    weapon += splitted[i] + " ";
                weapon = weapon.Trim();

                weapon = weapon.TrimEnd();
                if (weapon.EndsWith(".")) //THIS IS NOT A CRIT
                {
                    weapon = weapon.Remove(weapon.Length - 1);
                }
                else if (weapon.EndsWith("(crit)"))
                {
                    weapon = weapon.Replace(" (crit)", "");
                    weapon.Trim();
                    weapon.TrimEnd();
                    if (weapon.EndsWith("."))
                    {
                        weapon = weapon.Remove(weapon.Length - 1);
                    }
                }
             
                //Event
                Events.PlayerKill.Invoke(killer, killed, weapon);
                //Event
            }
            else if (s.Contains("connected") && !s.Contains(":"))
            {
                s = s.Trim();
                var splitted = s.Split(null);
                var connectedselector = 0;
                for (var i = 0; i < splitted.Length; i++)
                    if (splitted[i] == "connected")
                        connectedselector = i;
                var joined = "";
                if (connectedselector == 0)
                    return;
                for (var i = 0; i < connectedselector; i++)
                    joined += splitted[i] + " ";
                joined = joined.Trim();
                joined = joined.TrimEnd();
                Events.PlayerConnect.Invoke(joined);
                //Event
            }
            else if (s.Contains("suicided.") && !s.Contains(":"))
            {
                s = s.Trim();
                var splitted = s.Split(null);
                var connectedselector = 0;
                for (var i = 0; i < splitted.Length; i++)
                    if (splitted[i] == "suicided.")
                        connectedselector = i;
                var suicided = "";
                if (connectedselector == 0)
                    return;
                for (var i = 0; i < connectedselector; i++)
                    suicided += splitted[i] + " ";
                suicided = suicided.Trim();
                suicided = suicided.TrimEnd();
                Events.PlayerSuicide.Invoke(suicided);
                //Event
            }
        }
    }
}