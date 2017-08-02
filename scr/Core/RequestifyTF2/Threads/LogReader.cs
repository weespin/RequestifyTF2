using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
            thread.Start();
        }

        public static void Read()
        {
            var wh = new AutoResetEvent(false);
            var fsw = new FileSystemWatcher(".")
            {
                Filter = Instances.Config.GameDir + "/console.log",
                EnableRaisingEvents = true
            };
            fsw.Changed += (s, e) => wh.Set();
            if (!File.Exists(Instances.Config.GameDir + "/console.log"))
                File.Create(Instances.Config.GameDir + "/console.log");
            var fs = new FileStream(Instances.Config.GameDir + "/console.log", FileMode.Open, FileAccess.Read,
                FileShare.ReadWrite);
            using (var sr = new StreamReader(fs))
            {
                var s = "";
                while (true)
                {
                    s = sr.ReadLine();
                    if (!string.IsNullOrEmpty(s))
                    {
                        TextChecker(s);
                    }
                    else
                    {
                        wh.WaitOne(30);
                    }
                }
            }
        }

        public static void TextChecker(string s)
        {
        
            if (s.Contains(":") && s.Split(null).Length > 3)
            {
                s = s.Trim();
                var splitted = s.Split(null);
                //lets find :?
                var selector = 0;
                for (int i = 0; i < splitted.Length; i++)
                {
                    if (splitted[i] == ":")
                    {
                        selector = i;
                    }
                }
                var name = "";
                if (selector == 0)
                {
                    //No user -> return!
                    return;
                }
                for (int i = 0; i < selector; i++)
                {
                    name += splitted[i];
                }
                List<string> arguments = new List<string>();
                if (splitted.Length > selector + 2)
                {
                    var command = splitted[selector + 2];
                   
                    if (splitted.Length > selector + 3)
                    {
                        for (int i = selector + 3; i < splitted.Length; i++)
                        {
                            arguments.Add(splitted[i]);
                        }
                    }
                   Executer.Execute(name,command,arguments);
                }
                else
                {
                    return;
                }
            }
            else if (s.Contains("killed") && s.Contains("with") && s.EndsWith("."))
            {

                s = s.Trim();
                var splitted = s.Split(null);
                //its 100% kill
                //Lets find killer!
                var killerselector = 0;
                for (int i = 0; i < splitted.Length; i++)
                {
                    if (splitted[i] == "killed")
                    {
                        killerselector = i;
                    }
                }
                var killer = "";
                if (killerselector == 0)
                {
                    //No user -> return!
                    return;
                }
                for (int i = 0; i < killerselector; i++)
                {
                    killer += splitted[i] + " ";
                }
                killer = killer.Trim();
                killer = killer.TrimEnd();
                //Okay we found a killer, lets find which guy was killed?
                var deathselector = 0;
                for (int i = 0; i < splitted.Length; i++)
                {
                    if (splitted[i] == "with")
                    {
                        deathselector = i;
                    }
                }
                var killed = "";
                if (deathselector == 0)
                {
                    //No user -> return!
                    return;
                }
                for (int i = killerselector + 1; i < deathselector; i++)
                {
                    killed += splitted[i] + " ";
                }
                killed = killed.Trim();
                killed = killed.TrimEnd();
                string weapon = "";
                for (int i = deathselector + 1; i < splitted.Length; i++)
                {
                    weapon += splitted[i] + " ";
                }
                weapon = weapon.Trim();

                weapon = weapon.TrimEnd();
                weapon = weapon.Remove(weapon.Length - 1);
               //Event

            }
            else if (s.Contains("connected") && !s.Contains(":"))
            {
                s = s.Trim();
                var splitted = s.Split(null);
                var connectedselector = 0;
                for (int i = 0; i < splitted.Length; i++)
                {
                    if (splitted[i] == "connected")
                    {
                        connectedselector = i;
                    }
                }
                var joined = "";
                if (connectedselector == 0)
                {
                    //No user -> return!
                    return;
                }
                for (int i = 0; i < connectedselector; i++)
                {
                    joined += splitted[i] + " ";
                }
                joined = joined.Trim();
                joined = joined.TrimEnd();
                //Event
            }
            else if (s.Contains("suicided.") && !s.Contains(":"))
            {
                s = s.Trim();
                var splitted = s.Split(null);
                var connectedselector = 0;
                for (int i = 0; i < splitted.Length; i++)
                {
                    if (splitted[i] == "suicided.")
                    {
                        connectedselector = i;
                    }
                }
                var suicided = "";
                if (connectedselector == 0)
                {
                    //No user -> return!
                    return;
                }
                for (int i = 0; i < connectedselector; i++)
                {
                    suicided += splitted[i] + " ";
                }
                suicided = suicided.Trim();
                suicided = suicided.TrimEnd();
                //Event

            }

        }
    }
}