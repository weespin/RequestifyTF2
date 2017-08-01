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
                    var mute = false;
                    s = sr.ReadLine();

                    if (s != null && s.Contains("!"))
                    {
                        foreach (var plugin in Instances.ActivePlugins)
                        {
                            var gg = s.Split(null);
                            foreach (var shit in Instances.Config.Ignored)
                            {
                                if (Instances.Config.IgnoredReversed)
                                {
                                    if (!s.Contains(shit))
                                    {
                                        mute = true;
                                    }
                                 
                                }
                                else
                                {
                                    if (s.Contains(shit))
                                    {
                                        mute = true;
                                    }
                                }
                            }

                            if (s.Contains(plugin.Command))
                            {
                                if (!mute)
                                {
                                    if (Instances.Config.OnlyAdmin)
                                    {
                                        if (s.Contains(Instances.Config.Chiper))
                                        {
                                            plugin.Execute(s.Split(null));
                                            Instances.Config.Chiper = new CodeGenerator().GenerateWord(4);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (plugin.OnlyCode && s.Contains(Instances.Config.Chiper))
                                        {
                                            plugin.Execute(s.Split(null));
                                            Instances.Config.Chiper = new CodeGenerator().GenerateWord(4);
                                            break;
                                        }
                                        if (!plugin.OnlyCode)
                                        {
                                            plugin.Execute(s.Split(null));
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        
                        foreach (var plugin in Instances.DisabledPlugins)
                        {
                            var gg = s.Split(null);
                            foreach (var shit in Instances.Config.Ignored)
                            {
                                if (Instances.Config.IgnoredReversed)
                                {
                                    if (!s.Contains(shit))
                                    {
                                        mute = true;
                                    }

                                }
                                else
                                {
                                    if (s.Contains(shit))
                                    {
                                        mute = true;
                                    }
                                }
                            }
                            if (!mute)
                            {
                                if (s.Contains(plugin.Command))
                                    ConsoleSender.SendCommand("[RequestifyTF2] Sorry but " + plugin.Command + " plugin was disabled.",
                                        ConsoleSender.Command.Chat);
                            }
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