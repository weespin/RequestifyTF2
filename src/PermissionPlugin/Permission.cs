// PermissionPlugin
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequestifyTF2.API;
using RequestifyTF2.API.ConsoleAPI;
using RequestifyTF2.API.Permission;

namespace PermissionPlugin
{
    public class PermissionPlugin : IRequestifyPlugin
    {
        public string Author => "Weespin";
        public string Name => "Permissions";
        public string Desc => "Managing permission using game chat";

        public class PermissionCommand : IRequestifyCommand
        {
            public string Help => "Command to get permission level from group";
            public string Name => "permission";
            public Rules Permission => Rules.Assign;
            public List<string> Alias => new List<string>(){"per"};
            public void Execute(User executor, List<string> arguments)
            {
                if (executor.Group == Group.Admin || executor.Group == Group.Moderator)
                {
                    if (arguments.Count == 0)
                    {
                        ConsoleSender.SendCommand("Use !permission {name}",ConsoleSender.Command.Chat);
                    }
                    else
                    {
                        var username = String.Join(" ", arguments.ToArray()).Trim();
                        if (Permissions.Exists(username))
                        {
                            ConsoleSender.SendCommand(Permissions.GetGroup(username).ToString(),ConsoleSender.Command.Chat);
                        }
                    }
                }
            }
        }
        public class GroupCommand : IRequestifyCommand
        {
            public string Help => "Managing users";
            public string Name => "group";
            public Rules Permission => Rules.Assign;
            public List<string> Alias => new List<string>() { "per" };
            public void Execute(User executor, List<string> arguments)
            {
                if (executor.Group == Group.Admin || executor.Group==Group.Moderator)
                {
                    if (arguments.Count >2)
                    {
                        ConsoleSender.SendCommand("Use !group add {groupname} {nickname}", ConsoleSender.Command.Chat);
                    }
                    else
                    {
                        var username = String.Join(" ", arguments.Skip(1).ToArray()).Trim();
                        if (arguments[0] == "up")
                        {

                            if (Permissions.Exists(username))
                            {
                                if (Permissions.GetGroup(username) == Group.DJ && executor.Group == Group.Moderator)
                                {
                                    ConsoleSender.SendCommand("You can't set moderators.", ConsoleSender.Command.Chat);
                                    return;
                                }
                            }
                            if (Permissions.RnkUp(username))
                            {
                                ConsoleSender.SendCommand($"Done, now {username} group is {Permissions.GetGroup(username)}", ConsoleSender.Command.Chat);
                            }
                            else
                            {
                                if (Permissions.Exists(username))
                                {
                                    ConsoleSender.SendCommand("Can't do this.", ConsoleSender.Command.Chat);
                                }
                                else
                                {
                                    ConsoleSender.SendCommand("User don't exist in database", ConsoleSender.Command.Chat);
                                }
                            }
                        }

                        if (arguments[0] == "down")
                        {
                            if (Permissions.Exists(username))
                            {
                                if (Permissions.GetGroup(username) == Group.Admin && executor.Group == Group.Moderator)
                                {
                                    ConsoleSender.SendCommand("You can't derank Admins.", ConsoleSender.Command.Chat);
                                }
                            }
                            if (Permissions.RnkDn(username))
                            {
                                ConsoleSender.SendCommand($"Done, now {username} group is {Permissions.GetGroup(username)}", ConsoleSender.Command.Chat);
                            }
                            else
                            {
                                if (Permissions.Exists(username))
                                {
                                    ConsoleSender.SendCommand("Can't do this.", ConsoleSender.Command.Chat);
                                }
                                else
                                {
                                    ConsoleSender.SendCommand("User don't exist in database", ConsoleSender.Command.Chat);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
