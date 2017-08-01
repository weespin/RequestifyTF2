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
        }
    }
}