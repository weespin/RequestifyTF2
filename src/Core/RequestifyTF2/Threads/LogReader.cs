using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using RequestifyTF2.API;
using RequestifyTF2.API.Events;
using RequestifyTF2.Commands;
using RequestifyTF2.Utils;

namespace RequestifyTF2
{
    public class ReaderThread
    {
       
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

        public static readonly Regex CommandRegex = new Regex(@"^(.+) : (.+)$");

        public static readonly Regex ConnectRegex = new Regex(@"^(.+)(connected)$"); //todo: localize this

        public static readonly Regex KillRegex = new Regex(@"^(.+) killed (.+) with (.+)\.( \(crit\))?$");
         
        public static readonly Regex SuicideRegex = new Regex(@"^(.+) (suicided)\.$");
 
        public static void Read()
        {
            var wh = new AutoResetEvent(false);
            var fsw = new FileSystemWatcher(".")
            {
                Filter = Instance.GameDir + "/console.log",
                EnableRaisingEvents = true
            };
            fsw.Changed += (s, e) => wh.Set();
            if (!File.Exists(Instance.GameDir + "/console.log"))
            {
                File.Create(Instance.GameDir + "/console.log");
            }

            Thread.Sleep(30);


            var fs = new FileStream(
                Instance.GameDir + "/console.log",
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite);
           
            using (var sr = new StreamReader(fs, Encoding.GetEncoding("UTF-8")))
            {
                sr.ReadToEnd();
                var s = string.Empty;
                while (true)
                {
                    s = sr.ReadLine();
                    if (!string.IsNullOrEmpty(s))
                    {
                        TextChecker(s);
                    }
                    else
                    {
                        wh.WaitOne(25);
                    }
                }
            }
           
        }
        private static Thread thread;
        public static void StopThread()
        {
            if (thread.IsAlive)
            {
                thread.Abort();
            }
        }
        public static void StartThread()
        {
            thread = new Thread(Read) {IsBackground = true};
            thread.Start();
            Logger.Write(Logger.Status.Info, Localization.Localization.CORE_STARTED_LOGREADER_THREAD);
        }

        public static Result TextChecker(string s)
        {
            if (CommandRegex.Match(s).Success && s.Split(null).Length > 3)
            {
                var reg = CommandRegex.Match(s);
              
                var arguments = new List<string>();
                var split = reg.Groups[2].Value.Trim(null).Split(null);



                if (split.Length > 0)
                {
                    if (split[0].StartsWith("!"))
                    {
                        if (split.Length > 1)
                        {
                            for (var i = 1; i < split.Length; i++)
                            {
                                arguments.Add(split[i]);
                            }
                        }

                        Executer.Execute(ProcessUser(reg.Groups[1].Value), split[0], arguments);
                        Statisctics.CommandsParsed++;
                        return Result.CommandExecute;
                    }
                    else
                    {
                    
                        
                        Events.PlayerChat.Invoke(ProcessUser(reg.Groups[1].Value), reg.Groups[2].Value.Trim(null));
                        return Result.Chatted;
                    }
                }


            }

            if (KillRegex.Match(s).Success)
            {
                var reg = KillRegex.Match(s);
                var killer = reg.Groups[1].Value;
                var killed = reg.Groups[2].Value;
                var weapon = reg.Groups[3].Value;
                var crit = reg.Groups[4].Value.Contains("(crit)");
                if (!crit)
                {
                    // THIS IS NOT A CRIT
                    if (killer == Instance.Admin)
                    {
                        Statisctics.YourKills++;
                    }
                    else
                    {
                        Statisctics.GameKills++;
                    }

                    if (killed == Instance.Admin)
                    {
                        Statisctics.YourDeaths++;
                    }
                    else
                    {
                        Statisctics.Deaths++;
                    }

                    Events.PlayerKill.Invoke(killer, killed, weapon);
                    return Result.Kill;
                }

                if (killer == Instance.Admin)
                {
                    Statisctics.YourCritsKill++;
                }
                else
                {
                    Statisctics.CritsKill++;
                }

                if (killed == Instance.Admin)
                {
                    Statisctics.YourDeaths++;
                }
                else
                {
                    Statisctics.Deaths++;
                }

                Events.PlayerKill.Invoke(killer, killed, weapon, true);
                return Result.KillCrit;
            }

            if (ConnectRegex.Match(s).Success && !s.Contains(":"))
            {
                Statisctics.ConnectedPlayers++;
                var reg = ConnectRegex.Match(s);
                Events.PlayerConnect.Invoke(reg.Groups[1].Value);
                return Result.Connected;
            }

            if (SuicideRegex.Match(s).Success && !s.Contains(":"))
            {
                var reg = SuicideRegex.Match(s);
                if (reg.Groups[1].Value == Instance.Admin)
                {
                    Statisctics.YourSuicides++;
                }
                else
                {
                    Statisctics.Suicides++;
                }

                Events.PlayerSuicide.Invoke(reg.Groups[1].Value);
                return Result.Suicide;
            }

            Events.UndefinedMessage.Invoke(s);
            Statisctics.LinesParsed++;
            return Result.Undefined;
        }

        public static User ProcessUser(string s)
        {
            var ret = new User();
            if (s.Contains(Localization.Localization.TF_CHAT_DEAD))
            {
                ret.Tag |= Tag.Dead;
            }

            if (s.Contains(Localization.Localization.TF_CHAT_TEAM))
            {
                ret.Tag |= Tag.Team;
            }

            if (s.Contains(Localization.Localization.TF_CHAT_SPECTATOR))
            {
                ret.Tag |= Tag.Spectator;
            }

            ret.Name = s.Replace(Localization.Localization.TF_CHAT_TEAM, "")
                .Replace(Localization.Localization.TF_CHAT_DEAD, "");
            return ret;
        }
    }
}