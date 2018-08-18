using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using RequestifyTF2.API;
using RequestifyTF2.API.Events;

namespace RequestifyTF2.Managers
{
    public static class CommandManager
    {
        public enum Status
        {
            Enabled,
            Disabled
        }

        private static readonly List<RequestifyCommand> Commands  = new List<RequestifyCommand>();

        static CommandManager()
        {
            Events.OnPluginLoaded += Events_OnPluginLoaded;
        }

        private static void Events_OnPluginLoaded(RequestifyEventArgs.PluginLoadedArgs e)
        {
            var assembly = PluginManager.GetAssembly(e.Plugin);
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

            [JsonIgnoreAttribute]
            public IRequestifyCommand ICommand { get; set; }

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

            public string Help => ICommand.Help;

            public string Name => ICommand.Name;

            public bool OnlyAdmin => ICommand.OnlyAdmin;

            public List<string> Alias => ICommand.Alias;
        }
    }
}