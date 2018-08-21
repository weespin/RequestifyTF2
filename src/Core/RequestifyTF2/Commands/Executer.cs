// RequestifyTF2
// Copyright (C) 2018  Villiam Nmerukini
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RequestifyTF2.API;
using RequestifyTF2.API.Events;
using RequestifyTF2.API.IgnoreList;
using RequestifyTF2.Managers;
using RequestifyTF2.Utils;

namespace RequestifyTF2.Commands
{
    static class Executer
    {
        public static void Execute(User caller, string command, List<string> arguments)
        {
            CommandManager.RequestifyCommand calledcommand = null;
            foreach (var n in CommandManager.GetCommands())
            {
                if (command == "!" + n.Name)
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
                {
                    argstostring = string.Join(" ", arguments.ToArray());
                }

                Events.PlayerChat.Invoke(caller, command + " " + argstostring);
            }
            else
            {
                if (!caller.Group.HasFlag(calledcommand.Permission))
                {
                    return;
                }

                if (PluginManager.GetPluginFromCommand(calledcommand).Status == PluginManager.Status.Disabled ||
                    calledcommand.Status == CommandManager.Status.Disabled)
                {
                    return;
                }

                if (!IgnoreList.Contains(caller.Name))
                {
                    if (!IgnoreList.Reversed)
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
                                    Logger.Nlogger.Error(e, "Can't run command?");
                                }
                            });
                        Logger.Nlogger.Info(Localization.Localization.CORE_EXECUTED_COMMAND, caller.Name, command);
                    }
                    else
                    {
                        Statisctics.IgnoreListStopped++;
                        Logger.Nlogger.Error(Localization.Localization.CORE_USER_BLACKLISTED_FOR_EXECUTING, caller.Name,
                            command);
                    }
                }
                else
                {
                    if (IgnoreList.Reversed)
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
                                    Logger.Nlogger.Error(e, "Can't run command?");
                                }
                            });
                        Logger.Nlogger.Info(Localization.Localization.CORE_USER_INVOKED, caller.Name, command);
                    }
                    else
                    {
                        Statisctics.IgnoreListStopped++;
                        Logger.Nlogger.Info(Localization.Localization.CORE_USER_BLACKLISTED_FOR_EXECUTING, caller.Name,
                            command);
                    }
                }
            }
        }
    }
}