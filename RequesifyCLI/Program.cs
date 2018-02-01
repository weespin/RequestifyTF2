using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RequestifyTF2;
using RequestifyTF2.Api;

namespace RequesifyCLI
{
    internal class Program
    {
        private static bool started;

        private static void Main(string[] args)
        {
            var _plugins = new Dictionary<string, IRequestifyPlugin>();
            if (!Directory.Exists(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/plugins/")
            )
                Directory.CreateDirectory(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/plugins/");
            Instance.Config.AhkPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) +
                                      "/plugins/ahk/ahk.exe";

            var plugins =
                PluginLoader<IRequestifyPlugin>.LoadPlugins(
                    Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) +
                    "/plugins/");

            foreach (var item in plugins)
            {
                Instance.ActivePlugins.Add(item);
                _plugins.Add(item.Name, item);
            }

            AppConfig.Load();
            GetHelp();
            PrintPlugins();
            while (true)
            {
                var key = Console.ReadLine();
                if (key.StartsWith("reverse"))
                {
                    if (!Instance.Config.IgnoredReversed)
                    {
                        Logger.Write(Logger.Status.Info,"WhiteList activated");
                    }
                    else
                    {
                        Logger.Write(Logger.Status.Info,"BlackList activated");
                    }

                    Instance.Config.IgnoredReversed = !Instance.Config.IgnoredReversed;
                }

                if (key.StartsWith("help"))
                {
                   GetHelp();
                }
                if (key.StartsWith("dir") && key.Split(null).Length > 1) SetDirectory(key.Replace("dir", null));
                if (key.StartsWith("code")) Console.WriteLine(Instance.Config.Chiper);
                if (key.StartsWith("blacklist")) PrintBlackList();
                if (key.StartsWith("remove"))
                    if (key.Split(null).Length > 1)
                    {
                        var i = 0;
                        if (int.TryParse(key.Split()[1], out i))
                        {
                            var allplg = Instance.Config.Ignored;
                            if (i >= 0 && i < allplg.Count)
                            {
                                var plz = allplg[i];
                                if (plz == null) continue;

                                Instance.Config.Ignored.Remove(plz);
                                PrintBlackList();
                            }
                            else
                            {
                                Console.WriteLine($"Error. You dont have {i} blacklisted words");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error. Cant find any number");
                        }
                    }
                if (key.StartsWith("add"))
                {
                    var temp = key.Split(null).ToList();
                    temp.RemoveAt(0);
                    var res =
                    String.Join<string>(String.Empty, temp);
                    Instance.Config.Ignored.Add(res);
                    PrintBlackList();
                }
                if (key.StartsWith("list")) PrintPlugins();
                if (key.StartsWith("switch"))
                    if (key.Split(null).Length > 1)
                    {
                        var i = 0;
                        if (int.TryParse(key.Split()[1], out i))
                        {
                            var allplg = GetAllPlugins();
                            if (i >= 0 && i < allplg.Count)
                            {
                                var plz = allplg[i];
                                if (plz == null) continue;

                                if (Instance.DisabledPlugins.Contains(plz))
                                {
                                    Instance.ActivePlugins.Add(plz);
                                    Instance.DisabledPlugins.Remove(plz);
                                }

                                if (Instance.ActivePlugins.Contains(plz))
                                {
                                    Instance.DisabledPlugins.Add(plz);
                                    Instance.ActivePlugins.Remove(plz);
                                }

                                PrintPlugins();
                            }
                            else
                            {
                                Console.WriteLine($"Error. You have only {GetAllPlugins().Count} plugins. Not {i}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error. Cant find any number");
                        }
                    }

                if (key.StartsWith("start"))
                {
                    if (Instance.Config.GameDir == "")
                    {
                        Console.WriteLine("Please set the game directory");
                        return;
                    }

                    if (!started)
                    {
                        Runner.Start();
                        //     btn_start.Enabled = false;
                        started = true;

                        var s = _plugins.Aggregate("", (current, plugin) => current + plugin.Value.Name);
                    }

                    Console.WriteLine("Started");
                }
            }
        }

        public static void GetHelp()
        {
            Logger.Write(Logger.Status.Info,"RequestifyTF2 CLI");
            Logger.Write(Logger.Status.Info,"dir {path} - set directory");
            Logger.Write(Logger.Status.Info,"code - return code");
            Logger.Write(Logger.Status.Info,"blacklist - Print BlackLists");
            Logger.Write(Logger.Status.Info,"remove {id} - delete from blacklist");
            Logger.Write(Logger.Status.Info,"add {text} - add text to blacklist");
            Logger.Write(Logger.Status.Info,"list - return plugin list");
            Logger.Write(Logger.Status.Info,"switch {id} - disable/enable plugin");
            Logger.Write(Logger.Status.Info,"start - start RequestifyTF2");
            Logger.Write(Logger.Status.Info,"reverse - revert blacklist to whitelist");
            Logger.Write(Logger.Status.Info,"help - get this info.");
        }
        public static List<IRequestifyPlugin> GetAllPlugins()
        {
            var Plugins = new List<IRequestifyPlugin>();
            foreach (var pl in Instance.ActivePlugins) Plugins.Add(pl);
            foreach (var pl in Instance.DisabledPlugins) Plugins.Add(pl);
            Plugins.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
            return Plugins;
        }

        private static void PrintBlackList()
        {
            var i = 0;
            var blacklisted = Instance.Config.Ignored.OrderBy(n => n).ToList();
            Console.WriteLine("===================BLACKLIST===================");
            foreach (var blocked in blacklisted)
            {
                Console.WriteLine($"{{{i}}} {blocked}");
                i++;
            }

            Console.WriteLine("===================BLACKLIST END===================");
        }

        private static void PrintPlugins()
        {
            var i = 0;
            var Plugins = GetAllPlugins();
            Console.WriteLine("===================PLUGINS===================");
            foreach (var pl in Plugins)
            {
                if (Instance.DisabledPlugins.Contains(pl)) Console.ForegroundColor = ConsoleColor.Red;

                if (Instance.ActivePlugins.Contains(pl)) Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine($"{{{i}}} {pl.Name} {pl.Command} - {pl.Help} by {pl.Author}");
                i++;
            }

            ;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("===================PLUGINS END===================");
        }

        private static void SetDirectory(string v)
        {
            var dirs = Directory.GetDirectories(v);

            if (dirs.Any(n => n.Contains("cfg")))
            {
                AppConfig.CurrentConfig.GameDirectory = v;
                Logger.Write(Logger.Status.Info,"Current game path: " + v);
                AppConfig.Save();
            }
            else
            {
                foreach (var dir in dirs)
                {
                    var cdir = Directory.GetDirectories(dir);
                    var bin = false;
                    var cfg = false;
                    foreach (var dirz in cdir)
                    {
                        var pal = dirz;
                        var z = pal.Remove(0, dir.Length);

                        if (z.Contains("cfg")) cfg = true;
                        if (z.Contains("bin")) bin = true;
                        if (bin && cfg)
                        {
                            AppConfig.CurrentConfig.GameDirectory = dir;
                            Logger.Write(Logger.Status.Info,
                                $"Game path was automatically corrected from \n{v}\nto\n{dir}");
                            Logger.Write(Logger.Status.Info,"Current game path: " + dir);
                            AppConfig.Save();
                            return;
                        }
                    }
                }

                Logger.Write(Logger.Status.Error,
                    "Cant find cfg folder.. \nMaybe its not a game folder? \nIf its CSGO pick 'csgo' folder, if TF2 pick 'tf2' folder, ect.",ConsoleColor.Red);
            }
        }
    }
}