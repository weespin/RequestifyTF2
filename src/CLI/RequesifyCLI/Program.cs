using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RequestifyTF2;
using RequestifyTF2.Api;
using RequestifyTF2.Managers;

namespace RequesifyCLI
{
    internal class Program
    {
        private static bool started;

        public static List<PluginManager.Plugin> GetAllPlugins()
        {
            var Plugins = new List<PluginManager.Plugin>();
            foreach (var pl in Instance.Plugins.GetPlugins()) Plugins.Add(pl);

            Plugins.Sort((a, b) => string.Compare(a.plugin.Name, b.plugin.Name, StringComparison.Ordinal));
            return Plugins;
        }

        public static void GetHelp()
        {
            Logger.Write(Logger.Status.Info, "RequestifyTF2 CLI");
            Logger.Write(Logger.Status.Info, "dir {path} - set directory");
            Logger.Write(Logger.Status.Info, "code - return code");
            Logger.Write(Logger.Status.Info, "blacklist - Print BlackLists");
            Logger.Write(Logger.Status.Info, "remove {id} - delete from blacklist");
            Logger.Write(Logger.Status.Info, "add {text} - add text to blacklist");
            Logger.Write(Logger.Status.Info, "list - return plugin list");
            Logger.Write(Logger.Status.Info, "switch {id} - disable/enable plugin");
            Logger.Write(Logger.Status.Info, "start - start RequestifyTF2");
            Logger.Write(Logger.Status.Info, "reverse - revert blacklist to whitelist");
            Logger.Write(Logger.Status.Info, "help - get this info.");
            Logger.Write(Logger.Status.Info, "admin {nick} - set admin name");
            Logger.Write(Logger.Status.Info, "mute - mute or unmute chat");
        }

        private static void Main(string[] args)
        {
            var _plugins = new Dictionary<string, IRequestifyPlugin>();
            if (!Directory.Exists(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/plugins/"))
                Directory.CreateDirectory(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/plugins/");

            Instance.Plugins.loadPlugins(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/plugins/");


            AppConfig.Load();
            GetHelp();
            //PrintPlugins();
            while (true)
            {
                var key = Console.ReadLine();

                if (key.StartsWith("reverse"))
                {
                    if (!Instance.Config.IgnoredReversed)
                        Logger.Write(Logger.Status.Info, "WhiteList activated");
                    else
                        Logger.Write(Logger.Status.Info, "BlackList activated");

                    Instance.Config.IgnoredReversed = !Instance.Config.IgnoredReversed;
                }

                if (key.StartsWith("help")) GetHelp();

                if (key.StartsWith("dir") && key.Split(null).Length > 1) SetDirectory(key.Replace("dir", null));
                if (key.StartsWith("admin") && key.Split(null).Length > 1)
                {
                    Instance.Config.Admin = key.Replace("admin", null);
                    AppConfig.CurrentConfig.Admin = key.Replace("admin", null);
                    AppConfig.Save();
                }


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
                                Logger.Write(Logger.Status.Error, $"Error. You dont have {i} blacklisted words");
                            }
                        }
                        else
                        {
                            Logger.Write(Logger.Status.Error, $"Cant find any number");
                        }
                    }

                if (key.StartsWith("add"))
                {
                    var temp = key.Split(null).ToList();
                    temp.RemoveAt(0);
                    var res = string.Join<string>(string.Empty, temp);
                    Instance.Config.Ignored.Add(res);
                    PrintBlackList();
                }

                if (key.StartsWith("list")) PrintPlugins();

                if (key.StartsWith("mute"))
                {
                    if (Instance.isMuted)
                    {
                        Logger.Write(Logger.Status.Info, $"RequestifyTF2 is now unmuted");
                        Instance.isMuted = false;
                    }
                    else
                    {
                        Logger.Write(Logger.Status.Info, $"RequestifyTF2 is now muted");
                        Instance.isMuted = true;
                    }
                }

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

                                var pl = Instance.Plugins.GetPlugins().FirstOrDefault(n => n == plz);
                                if (pl != null)
                                {
                                    if (pl.Status == PluginManager.Status.Enabled)
                                        Instance.Plugins.DisablePlugin(pl);
                                    else
                                        Instance.Plugins.EnablePlugin(pl);
                                }
                            }
                            else
                            {
                                Logger.Write(
                                    Logger.Status.Error,
                                    $"Error. You have only {GetAllPlugins().Count} plugins. Not {i}");
                            }
                        }
                        else
                        {
                            Logger.Write(Logger.Status.Error, $"Cant find any number");
                        }
                    }

                if (key.StartsWith("start"))
                {
                    if (Instance.Config.GameDir == string.Empty)
                    {
                        Logger.Write(Logger.Status.Info, "Please set the game directory");
                        return;
                    }

                    if (!started)
                    {
                        Runner.Start();
                        started = true;

                        var s = _plugins.Aggregate(string.Empty, (current, plugin) => current + plugin.Value.Name);
                    }
                }
            }
        }

        private static void PrintBlackList()
        {
            var i = 0;
            var blacklisted = Instance.Config.Ignored.OrderBy(n => n).ToList();
            Logger.Write(Logger.Status.Info, "===================BLACKLIST===================");
            foreach (var blocked in blacklisted)
            {
                Console.WriteLine($"{{{i}}} {blocked}");
                i++;
            }

            Logger.Write(Logger.Status.Info, "===================BLACKLIST END===================");
        }

        private static void PrintPlugins()
        {
            var i = 0;
            var Plugins = GetAllPlugins();
            Logger.Write(Logger.Status.Info, "===================PLUGINS===================");
            foreach (var pl in Plugins)
            {
                if (pl.Status == PluginManager.Status.Disabled) Console.ForegroundColor = ConsoleColor.Red;

                if (pl.Status == PluginManager.Status.Enabled) Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine($"{{{i}}} {pl.plugin.Name} {pl.plugin.Name}  by {pl.plugin.Author}");
                i++;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Logger.Write(Logger.Status.Info, "===================PLUGINS END===================");
        }

        private static void SetDirectory(string v)
        {
            var dirs = Directory.GetDirectories(v);

            if (dirs.Any(n => n.Contains("cfg")))
            {
                AppConfig.CurrentConfig.GameDirectory = v;
                Logger.Write(Logger.Status.Info, "Current game path: " + v);
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
                            Logger.Write(
                                Logger.Status.Info,
                                $"Game path was automatically corrected from \n{v}\nto\n{dir}");
                            Logger.Write(Logger.Status.Info, "Current game path: " + dir);
                            AppConfig.Save();
                            return;
                        }
                    }
                }

                Logger.Write(
                    Logger.Status.Error,
                    "Cant find cfg folder.. \nMaybe its not a game folder? \nIf its CSGO pick 'csgo' folder, if TF2 pick 'tf2' folder, ect.",
                    ConsoleColor.Red);
            }
        }
    }
}