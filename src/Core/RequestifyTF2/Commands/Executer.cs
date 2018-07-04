using RequestifyTF2.Managers;
using RequestifyTF2.Utils;

namespace RequestifyTF2.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RequestifyTF2.Api;

    internal class Executer
    {
        public static void Execute(string caller, string command, List<string> arguments)
        {
            CommandManager.RequestifyCommand calledcommand = null;
            foreach (var n in Instance.Commands.GetCommands())
            {
                if (command=="!"+n.Name)
                {
                    calledcommand = n;
                    continue;
                }

                if (n.Alias != null)
                {
                    foreach (var s in n.Alias)
                    {
                        if (command == "!" + s)
                        {
                            calledcommand = n;
                            break;

                        }
                    }
                }
            }
           
            if (calledcommand == null)
            {
                var argstostring = string.Empty;
                if (arguments.Count > 0)
                    argstostring = string.Join(" ", argstostring.ToArray());
                Events.PlayerChat.Invoke(caller, command + " " + argstostring);
            }
            else
            {
                if (Instance.Plugins.GetPluginFromCommand(calledcommand).Status == PluginManager.Status.Disabled||calledcommand.Status==CommandManager.Status.Disabled)
                {
                    return;
                }
                if (!Instance.Config.Ignored.Contains(caller))
                {
                    if (!Instance.Config.IgnoredReversed)
                    {
                        Task.Run(
                            () =>
                                {
                                    try
                                    {
                                        calledcommand.Execute(caller, arguments);
                                    }
                                    catch (Exception e)
                                    {
                                        Logger.Write(Logger.Status.Error, e.ToString());
                                    }
                                });
                        Logger.Write(Logger.Status.Info, $"{caller} executed {command}");
                    }
                    else
                    {
                        Statisctics.IgnoreListStopped++;
                        Logger.Write(Logger.Status.Info, $"{caller} was blacklisted to execute {command}");
                    }
                }
                else
                {
                    if (Instance.Config.IgnoredReversed)
                    {
                        Task.Run(
                            () =>
                                {
                                    try
                                    {
                                        
                                        calledcommand.Execute(caller, arguments);
                                    }
                                    catch (Exception e)
                                    {
                                        Logger.Write(Logger.Status.Error, e.ToString());
                                    }
                                });
                        Logger.Write(Logger.Status.Info, $"{caller} invoked {command}");
                    }
                    else
                    {
                        Statisctics.IgnoreListStopped++;
                        Logger.Write(Logger.Status.Info, $"{caller} was blacklisted to execute {command}");
                    }
                }
            }
        }
    }
}