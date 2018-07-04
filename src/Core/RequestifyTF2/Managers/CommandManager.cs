using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RequestifyTF2.Api;
using RequestifyTF2.Managers;

namespace RequestifyTF2.Managers
{



    public class CommandManager
    {

      private List<RequestifyCommand> Commands = new List<RequestifyCommand>();

        public CommandManager()
        {
            foreach (var Plugin in PluginManager.pluginAssemblies)
            {
                var CommTypes = GetTypesFromInterface(Plugin, "IRequestifyCommand");
                foreach (var type in CommTypes)
                {


                    var NewCommand = new RequestifyCommand(Plugin,
                        Activator.CreateInstance(type) as IRequestifyCommand,Status.Enabled);
                    if (Commands.Count(n => n.Name == NewCommand.Name) == 0)
                    {
                        Commands.Add(new RequestifyCommand(Plugin,
                            Activator.CreateInstance(type) as IRequestifyCommand,Status.Enabled));
                    }

                }
            }
        }

        public enum Status
        {
            Enabled,
            Disabled
        }
        public class RequestifyCommand : IRequestifyCommand
        {
            
            public IRequestifyCommand ICommand;
            public RequestifyCommand(Assembly father, IRequestifyCommand command,Status status)
            {
                Father = father;
                ICommand = command;
                Status = status;
            }

            public Status Status;
            public Assembly Father;

            public void Execute(string executor, List<string> arguments)
            {
                ICommand.Execute(executor, arguments);
            }

            public string Help
            {
                get { return ICommand.Help; }
            }

            public string Name
            {
                get { return ICommand.Name; }
            }

            public bool OnlyAdmin
            {
                get { return ICommand.OnlyAdmin; }
            }

            public List<string> Alias
            {
                get { return ICommand.Alias; }
            }
        }

        public static List<Type> GetTypesFromInterface(List<Assembly> assemblies, string interfaceName)
        {
            List<Type> allTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                allTypes.AddRange(GetTypesFromInterface(assembly, interfaceName));
            }

            return allTypes;
        }

        public static List<Type> GetTypesFromInterface(Assembly assembly, string interfaceName)
        {
            List<Type> allTypes = new List<Type>();
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {

                types = e.Types;
            }

            foreach (Type type in types.Where(t => t != null))
            {
                if (type.GetInterface(interfaceName) != null)
                {
                    allTypes.Add(type);
                }
            }

            return allTypes;
        }
        public void DisableCommand(RequestifyCommand command)
        {
            command.Status =CommandManager.Status.Disabled;
        }

        public void EnableCommand(RequestifyCommand command)
        {
            command.Status = CommandManager.Status.Disabled;
        }
        public List<RequestifyCommand> GetCommands()
        {
            return Commands;
        }
        public RequestifyCommand GetCommand(string name)
        {
            foreach (var p in Commands)
            {
                if (p.Name==name||p.Alias.Contains(name))
                {
                    return p;
                }
            }
            return null;
        }
        public RequestifyCommand GetCommand(RequestifyCommand command)
        {
            return GetCommands().FirstOrDefault(n => n.Name == command.Name && n.Alias == command.Alias);
          
        }
    }
}