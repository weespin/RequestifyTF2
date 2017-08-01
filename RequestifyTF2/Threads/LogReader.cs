using System.IO;
using System.Linq;
using System.Threading;
using RequestifyTF2.Api;

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

                    if (s != null && s.Contains("!"))
                    {
                        foreach (var plugin in Instances.ActivePlugins)
                        {
                            var gg = s.Split(null);
                            if (gg.Any(a => Instances.Config.Ignored.Contains(s)))
                            {
                                if (Instances.Config.IgnoredReversed)
                                    continue;
                                return;
                            }

                            if (s.Contains(plugin.Command))
                                if (plugin.OnlyCode && s.Contains(Instances.Config.Chiper))
                                    plugin.Execute(s.Split(null));
                                else if (!plugin.OnlyCode)
                                    plugin.Execute(s.Split(null));
                        }
                        foreach (var plugin in Instances.DisabledPlugins)
                        {
                            var gg = s.Split(null);
                            if (gg.Any(a => Instances.Config.Ignored.Contains(s)))
                            {
                                if (Instances.Config.IgnoredReversed)
                                    continue;
                                return;
                            }
                            if (s.Contains(plugin.Command))
                                ConsoleSender.SendCommand("Sorry but " + plugin.Command + " command is disabled",
                                    ConsoleSender.Command.Chat);
                        }
                    }
                    else
                    {
                        wh.WaitOne(30);
                    }
                }
            }
        }
    }
}