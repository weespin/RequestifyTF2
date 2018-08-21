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
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using RequestifyTF2.API;
using RequestifyTF2.API.Events;
using RequestifyTF2.API.Permission;

namespace RequestifyTF2.Managers
{
    public static class CommandManager
    {
        public enum Status
        {
            Enabled,
            Disabled
        }

        private static readonly List<RequestifyCommand> Commands = new List<RequestifyCommand>();

        static CommandManager()
        {
            Events.OnPluginLoaded += Events_OnPluginLoaded;
        }

        private static void Events_OnPluginLoaded(RequestifyEventArgs.PluginLoadedArgs e)
        {
            var assembly = e.Plugin.GetAssembly();
            var CommTypes = GetTypesFromInterface(assembly, "IRequestifyCommand");
            foreach (var type in CommTypes)
            {
                var NewCommand = new RequestifyCommand(assembly,
                    Activator.CreateInstance(type) as IRequestifyCommand, Status.Enabled);
                if (Commands.Count(n => n.Name == NewCommand.Name) == 0)
                {
                    Commands.Add(NewCommand);
                    Events.CommandRegistered.Invoke(NewCommand);
                }
            }
        }

        public static List<Type> GetTypesFromInterface(List<Assembly> assemblies, string interfaceName)
        {
            var allTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                allTypes.AddRange(GetTypesFromInterface(assembly, interfaceName));
            }

            return allTypes;
        }

        public static void Init()
        {
            Logger.Nlogger.Debug("CommandManager Init");
        }

        public static List<Type> GetTypesFromInterface(Assembly assembly, string interfaceName)
        {
            var allTypes = new List<Type>();
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                types = e.Types;
            }

            foreach (var type in types.Where(t => t != null))
            {
                if (type.GetInterface(interfaceName) != null)
                {
                    allTypes.Add(type);
                }
            }

            return allTypes;
        }

        public static void Disable(this RequestifyCommand command)
        {
            command.Status = Status.Disabled;
        }

        public static void Enable(this RequestifyCommand command)
        {
            command.Status = Status.Disabled;
        }

        public static List<RequestifyCommand> GetCommands()
        {
            return Commands;
        }

        public static RequestifyCommand GetCommand(string name)
        {
            foreach (var p in Commands)
            {
                if (p.Name == name || p.Alias.Contains(name))
                {
                    return p;
                }
            }

            return null;
        }

        public static RequestifyCommand GetCommand(RequestifyCommand command)
        {
            return GetCommands().FirstOrDefault(n => n.Name == command.Name && n.Alias == command.Alias);
        }

        public class RequestifyCommand : IRequestifyCommand
        {
            [JsonIgnore] public Assembly Father { get; set; }

            [JsonIgnore] public IRequestifyCommand ICommand { get; set; }

            public Status Status { get; set; }

            public RequestifyCommand(Assembly father, IRequestifyCommand command, Status status)
            {
                Father = father;
                ICommand = command;
                Status = status;
            }

            public void Execute(User executor, List<string> arguments)
            {
                ICommand.Execute(executor, arguments);
            }

            public Rules Permission => ICommand.Permission;
            public string Help => ICommand.Help;

            public string Name => ICommand.Name;


            public List<string> Alias => ICommand.Alias;
        }
    }
}