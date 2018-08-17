using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RequestifyTF2;
using RequestifyTF2.API;
using RequestifyTF2.API.IgnoreList;
using RequestifyTF2.Managers;

namespace RequesifyCLI
{
    internal class Program
    {
    

        public static List<PluginManager.Plugin> GetAllPlugins()
        {
            var Plugins = new List<PluginManager.Plugin>();
            foreach (var pl in PluginManager.GetPlugins())
            {
                Plugins.Add(pl);
            }
            Plugins.Sort((a, b) => string.Compare(a.plugin.Name, b.plugin.Name, StringComparison.Ordinal));
            return Plugins;
        }

        public static void GetHelp()
        {
            Logger.Write(Logger.LogStatus.Info, "RequestifyTF2 CLI");
            Logger.Write(Logger.LogStatus.Info, "dir {path} - set directory");
            Logger.Write(Logger.LogStatus.Info, "code - return code");
            Logger.Write(Logger.LogStatus.Info, "blacklist - Print BlackLists");
            Logger.Write(Logger.LogStatus.Info, "remove {id} - delete from blacklist");
            Logger.Write(Logger.LogStatus.Info, "add {text} - add text to blacklist");
            Logger.Write(Logger.LogStatus.Info, "list - return plugin list");
            Logger.Write(Logger.LogStatus.Info, "switch {id} - disable/enable plugin");
            Logger.Write(Logger.LogStatus.Info, "start - start RequestifyTF2");
            Logger.Write(Logger.LogStatus.Info, "reverse - revert blacklist to whitelist");
            Logger.Write(Logger.LogStatus.Info, "help - get this info.");
            Logger.Write(Logger.LogStatus.Info, "admin {nick} - set admin name");
            Logger.Write(Logger.LogStatus.Info, "mute - mute or unmute chat");
        }

        private static void Main(string[] args)
        {
            AppConfig.Load();
            GetHelp();
            while (true)
            {
                var key = Console.ReadLine();

                if (key.StartsWith("reverse"))
                {
                    if (! IgnoreList.Reversed)
                    {
                        Logger.Write(Logger.LogStatus.Info, "WhiteList activated");
                    }
                    else
                    {
                        Logger.Write(Logger.LogStatus.Info, "BlackList activated");
                    }

                     IgnoreList.Reversed = ! IgnoreList.Reversed;
                }

                if (key.StartsWith("help"))
                {
                    GetHelp();
                }

                if (key.StartsWith("dir") && key.Split(null).Length > 1)
                {
                    SetDirectory(key.Replace("dir", null));
                }

                if (key.StartsWith("admin") && key.Split(null).Length > 1)
                {
                    Requestify.Admin = key.Replace("admin", null);
                    AppConfig.CurrentConfig.Admin = key.Replace("admin", null);
                    AppConfig.Save();
                }


                if (key.StartsWith("blacklist"))
                {
                    PrintBlackList();
                }

                if (key.StartsWith("remove"))
                {
                    if (key.Split(null).Length > 1)
                    {
                        var i = 0;
                        if (int.TryParse(key.Split()[1], out i))
                        {
                            var allplg =  IgnoreList.GetList;
                            if (i >= 0 && i < allplg.Count)
                            {
                                var plz = allplg[i];
                                if (plz == null)
                                {
                                    continue;
                                }

                                IgnoreList.Remove(plz);
                                PrintBlackList();
                            }
                            else
                            {
                                Logger.Write(Logger.LogStatus.Error, $"Error. You dont have {i} blacklisted words");
                            }
                        }
                        else
                        {
                            Logger.Write(Logger.LogStatus.Error, $"Cant find any number");
                        }
                    }
                }

                if (key.StartsWith("add"))
                {
                    var temp = key.Split(null).ToList();
                    temp.RemoveAt(0);
                    var res = string.Join<string>(string.Empty, temp);
                     IgnoreList.Add(res);
                    PrintBlackList();
                }

             if (key.StartsWith("list"))
                {
                    PrintPlugins();
                }

                if (key.StartsWith("mute"))
                {
                    if (Requestify.IsMuted)
                    {
                        Logger.Write(Logger.LogStatus.Info, $"RequestifyTF2 is now unmuted");
                        Requestify.IsMuted = false;
                    }
                    else
                    {
                        Logger.Write(Logger.LogStatus.Info, $"RequestifyTF2 is now muted");
                        Requestify.IsMuted = true;
                    }
                }

                if (key.StartsWith("switch"))
                {
                    if (key.Split(null).Length > 1)
                    {
                        var i = 0;
                        if (int.TryParse(key.Split()[1], out i))
                        {
                            var allplg = GetAllPlugins();
                            if (i >= 0 && i < allplg.Count)
                            {
                                var plz = allplg[i];
                                if (plz == null)
                                {
                                    continue;
                                }

                                var pl = PluginManager.GetPlugins().FirstOrDefault(n => n == plz);
                                if (pl != null)
                                {
                                    if (pl.Status == PluginManager.Status.Enabled)
                                    {
                                       pl.Disable();
                                    }
                                    else
                                    {
                                       pl.Enable();
                                    }
                                }
                            }
                            else
                            {
                                Logger.Write(
                                    Logger.LogStatus.Error,
                                    $"Error. You have only {GetAllPlugins().Count} plugins. Not {i}");
                            }
                        }
                        else
                        {
                            Logger.Write(Logger.LogStatus.Error, $"Cant find any number");
                        }
                    }
                }

                if (key.StartsWith("start"))
                {
                    if (Requestify.GameDir == string.Empty)
                    {
                        Logger.Write(Logger.LogStatus.Info, "Please set the game directory");
                        return;
                    }
                    Runner.Start();
                 

                  
                }
            }
        }

        private static void PrintBlackList()
        {
            var i = 0;
            var blacklisted =  IgnoreList.GetList.OrderBy(n => n).ToList();
            Logger.Write(Logger.LogStatus.Info, "===================BLACKLIST===================");
            foreach (var blocked in blacklisted)
            {
                Console.WriteLine($@"{{{i}}} {blocked}");
                i++;
            }

            Logger.Write(Logger.LogStatus.Info, "===================BLACKLIST END===================");
        }

        private static void PrintPlugins()
        {
            var i = 0;
            var Plugins = GetAllPlugins();
            Logger.Write(Logger.LogStatus.Info, "===================PLUGINS===================");
            foreach (var pl in Plugins)
            {
                if (pl.Status == PluginManager.Status.Disabled)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                if (pl.Status == PluginManager.Status.Enabled)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine($"{{{i}}} {pl.plugin.Name} {pl.plugin.Name}  by {pl.plugin.Author}");
                i++;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Logger.Write(Logger.LogStatus.Info, "===================PLUGINS END===================");
        }

        private static void SetDirectory(string v)
        {
            if (!Directory.Exists(v))
            {
                Logger.Write(Logger.LogStatus.Error,"This is not a directory");
                return;
            }
            var dirs = Directory.GetDirectories(v);

            if (dirs.Any(n => n.Contains("cfg")))
            {
                AppConfig.CurrentConfig.GameDirectory = v;
                Logger.Write(Logger.LogStatus.Info, "Current game path: " + v);
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

                        if (z.Contains("cfg"))
                        {
                            cfg = true;
                        }

                        if (z.Contains("bin"))
                        {
                            bin = true;
                        }

                        if (bin && cfg)
                        {
                            AppConfig.CurrentConfig.GameDirectory = dir;
                            Logger.Write(
                                Logger.LogStatus.Info,
                                $"Game path was automatically corrected from \n{v}\nto\n{dir}");
                            Logger.Write(Logger.LogStatus.Info, "Current game path: " + dir);
                            AppConfig.Save();
                            return;
                        }
                    }
                }

                Logger.Write(
                    Logger.LogStatus.Error,
                    "Cant find cfg folder.. \nMaybe its not a game folder? \nIf its CSGO pick 'csgo' folder, if TF2 pick 'tf2' folder, ect.",
                    ConsoleColor.Red);
            }
        }
    }
}