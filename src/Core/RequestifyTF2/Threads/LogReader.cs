namespace RequestifyTF2
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    using RequestifyTF2.Api;
    using RequestifyTF2.Commands;

    public class ReaderThread
    {
        public static Regex CommandRegex = new Regex(@"^(.+) : (.+)$");

        public static Regex ConnectRegex = new Regex(@"^(.+)(connected)$");

        public static Regex KillRegex = new Regex(@"^(.+) killed (.+) with (.+)\.( \(crit\))?$");

        public static Regex SuicideRegex = new Regex(@"^(.+) (suicided)\.$");

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
            var fs = new FileStream(
                Instance.Config.GameDir + "/console.log",
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite);
            using (var sr = new StreamReader(fs, Encoding.GetEncoding("Windows-1251")))
            {
                var s = string.Empty;
                while (true)
                {
                    s = sr.ReadLine();
                    if (!string.IsNullOrEmpty(s))
                    {
                        Encoding utf8 = Encoding.GetEncoding("UTF-8");
                        Encoding win1251 = Encoding.GetEncoding("Windows-1251");

                        byte[] utf8Bytes = win1251.GetBytes(s);
                        byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
                        s = win1251.GetString(win1251Bytes);
                        TextChecker(s);
                    }
                    else
                        wh.WaitOne(25);
                }
            }
        }

        public static void Starter()
        {
            var thread = new Thread(Read) { IsBackground = true };
            thread.Start();
            Logger.Write(Logger.Status.Info, "Started LogReader Thread!");
        }

        public static Result TextChecker(string s)
        {
            if (CommandRegex.Match(s).Success && s.Split(null).Length > 3)
            {
                var reg = CommandRegex.Match(s);
                var name = reg.Groups[1].Value;
                var arguments = new List<string>();
                var split = reg.Groups[2].Value.Trim(null).Split(null);
                if (split.Length > 0)
                {
                    if (split[0].StartsWith("!"))
                    {
                        if (split.Length > 1)
                        {
                            for (int i = 1; i < split.Length; i++)
                            {
                                arguments.Add(split[i]);
                            }
                        }

                        Executer.Execute(name, split[0], arguments);
                        return Result.CommandExecute;
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
                    Events.PlayerKill.Invoke(killer, killed, weapon);
                    return Result.Kill;
                }

                Events.PlayerKill.Invoke(killer, killed, weapon, true);
                return Result.KillCrit;
            }

            if (ConnectRegex.Match(s).Success && !s.Contains(":"))
            {
                var reg = ConnectRegex.Match(s);
                Events.PlayerConnect.Invoke(reg.Groups[1].Value);
                return Result.Connected;
            }

            if (SuicideRegex.Match(s).Success && !s.Contains(":"))
            {
                var reg = SuicideRegex.Match(s);
                Events.PlayerSuicide.Invoke(reg.Groups[1].Value);
                return Result.Suicide;
            }

            Events.UndefinedMessage.Invoke(s);
            return Result.Undefined;
        }
    }
}