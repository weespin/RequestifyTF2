using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using RequestifyTF2.Api;
using RequestifyTF2.Commands;

namespace RequestifyTF2
{
    public class ReaderThread
    {
        public static void Starter()
        {
            var thread = new Thread(Read) {IsBackground = true};
            thread.Start();
            Logger.Write(Logger.Status.Info, "Started LogReader Thread!");
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
            Thread.Sleep(30);
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
        //Added Enums for Unit Testing!
        public enum Result
        {
            CommandExecute,
            Kill,
            Suicide,
            Chatted,
            Connected,
            Undefined,
            KillCrit
        }

       
        public static Result TextChecker(string s)
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
                StringBuilder name = new StringBuilder();
                if (selector == 0)
                    return Result.Undefined;
                for (var i = 0; i < selector; i++)
                {
                    name.Append(splitted[i]);
                }
                var arguments = new List<string>();
                if (splitted.Length > selector + 1)
                {
                    var commandint = 0;
                    var command = "";
                    for (int i = selector+1; i < splitted.Length; i++)
                    {
                        if (splitted[i] != "")
                        {
                            command = splitted[i];
                            commandint = i;
                            break;
                        }
                    }
                   

                    if (splitted.Length > commandint+1)
                        for (var i = commandint+1; i < splitted.Length; i++)
                            arguments.Add(splitted[i]);
                    if (command != "")
                    {
                        Executer.Execute(name.ToString(), command, arguments);
                        return Result.CommandExecute;
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
                StringBuilder killer = new StringBuilder();
                if (killerselector == 0)
                    return Result.Undefined;
                for (var i = 0; i < killerselector; i++)
                {
                    killer.Append(splitted[i] + " ");
                }

                killer.Length--;
              
                //Okay we found a killer, lets find which guy was killed?
                var deathselector = 0;
                for (var i = 0; i < splitted.Length; i++)
                    if (splitted[i] == "with")
                        deathselector = i;
               StringBuilder killed = new StringBuilder();
                if (deathselector == 0)
                    return Result.Undefined;
                for (var i = killerselector + 1; i < deathselector; i++)
                    killed.Append(splitted[i] + " ");
                killed.Length--;
                
                var weapon = new StringBuilder();
                for (var i = deathselector + 1; i < splitted.Length; i++)
                    weapon.Append(splitted[i] + " ");
                weapon.Length--;

               
                if (weapon.ToString().EndsWith(".")) //THIS IS NOT A CRIT
                {
                    weapon.Length--;
                    Events.PlayerKill.Invoke(killer.ToString(), killed.ToString(), weapon.ToString());
                    return Result.Kill;
                }
                else if (weapon.ToString().EndsWith("(crit)"))
                {
                    weapon = weapon.Replace(" (crit)", "");
                    weapon.Length--;
                  
                    if (weapon.ToString().EndsWith("."))
                    {
                        weapon.Length--;
                    }
                    Events.PlayerKill.Invoke(killer.ToString(), killed.ToString(), weapon.ToString(),true);
                    return Result.KillCrit;
                }
             
                
            }
            else if (s.Contains("connected") && !s.Contains(":"))
            {
                s = s.Trim();
                var splitted = s.Split(null);
                var connectedselector = 0;
                for (var i = 0; i < splitted.Length; i++)
                    if (splitted[i] == "connected")
                        connectedselector = i;
                var joined = new StringBuilder();
                if (connectedselector == 0)
                    return Result.Undefined;
                for (var i = 0; i < connectedselector; i++)
                    joined.Append(splitted[i] + " ");
                joined.Length--;
                Events.PlayerConnect.Invoke(joined.ToString());
                return Result.Connected;
            }
            else if (s.Contains("suicided.") && !s.Contains(":"))
            {
                s = s.Trim();
                var splitted = s.Split(null);
                var connectedselector = 0;
                for (var i = 0; i < splitted.Length; i++)
                    if (splitted[i] == "suicided.")
                        connectedselector = i;
                var suicided = new StringBuilder();
                if (connectedselector == 0)
                    return Result.Undefined;
                for (var i = 0; i < connectedselector; i++)
                    suicided.Append(splitted[i] + " ");
                suicided.Length--;
                suicided.Length--;
                Events.PlayerSuicide.Invoke(suicided.ToString());
                return Result.Suicide;
            }

            return Result.Undefined;
        }
    }
}