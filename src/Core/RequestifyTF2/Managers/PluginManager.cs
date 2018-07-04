using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RequestifyTF2.Api;
using RequestifyTF2.PluginLoader;

namespace RequestifyTF2.Managers
{
    public class PluginManager
    {
        public PluginManager()
        {
            if (!Directory.Exists(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/plugins/"))
                Directory.CreateDirectory(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/plugins/");


            //          this.field_ignored.Enter += this.lbx_IgnoreList_Enter;
            loadPlugins(
                Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/");
        }
        public enum Status
        {
            Enabled,
            Disabled
        }

        public class Plugin
        {
            public Plugin(IRequestifyPlugin Plugin, Status status)
            {
                plugin = Plugin;
                Status = status;
            }

            public IRequestifyPlugin plugin;
            public Status Status;
        }

        private static List<Type> plugintypes = new List<Type>();
        public static List<Assembly> pluginAssemblies = new List<Assembly>();
        static List<Plugin> Plugins = new List<Plugin>();

        public static Dictionary<string, string> FindAllPlugins(string directory, string extension = "*.dll")
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            IEnumerable<FileInfo> dlls = new DirectoryInfo(directory).GetFiles(extension, SearchOption.AllDirectories);
            foreach (FileInfo file in dlls)
            {
                try
                {
                    AssemblyName name = AssemblyName.GetAssemblyName(file.FullName);
                    dict.Add(name.FullName, file.FullName);
                }
                catch
                {
                }
            }

            return dict;
        }

        public static List<Assembly> LoadAssembliesFromDirectory(string directory, string extension = "*.dll")
        {
            List<Assembly> assemblies = new List<Assembly>();
            IEnumerable<FileInfo> pluginsLibraries =
                new DirectoryInfo(directory).GetFiles(extension, SearchOption.AllDirectories);

            foreach (FileInfo library in pluginsLibraries)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(library.FullName);

                    List<Type> types = GetTypesFromInterface(assembly, "IRequestifyPlugin");

                    if (types.Count == 1)
                    {
                        Console.WriteLine("Loading " + assembly.GetName().Name + " from " + assembly.Location);
                        assemblies.Add(assembly);
                        Plugins.Add(new Plugin(Activator.CreateInstance(types[0]) as IRequestifyPlugin,
                            Status.Enabled));
                    }
                    else if (types.Count > 1)
                    {
                        Console.WriteLine("Invalid plugin: " + assembly.GetName().Name +
                                          "\n Plugin has more than 1 interface");
                    }
                    else

                    {
                        Console.WriteLine("Invalid plugin: " + assembly.GetName().Name);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cant load plugin: " + library.Name);
                    Console.WriteLine(ex);
                }
            }

            return assemblies;
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

        public void loadPlugins(string path)
        {
            Libraries.Load(Path.GetDirectoryName(Application.ExecutablePath) + "/lib/");
            var libraries = FindAllPlugins(path);
            if (libraries.Count == 0)
            {
                return;
            }

            foreach (KeyValuePair<string, string> pair in FindAllPlugins(path))
            {
                if (!libraries.ContainsKey(pair.Key))
                    libraries.Add(pair.Key, pair.Value);
            }

            pluginAssemblies = LoadAssembliesFromDirectory(path);
            List<Type> pluginImplemenations = GetTypesFromInterface(pluginAssemblies, "IRequestifyPlugin");
            foreach (Type pluginType in pluginImplemenations)
            {
                plugintypes.Add(pluginType);
            }

            foreach (var Assembly in pluginAssemblies)
            {
                var types = Assembly.GetTypes();
                foreach (var type in types)
                {
                    var m = type.GetMethod("OnLoad");
                    var plugin = Activator.CreateInstance(type);
                    if (m != null)
                    {
                        Task.Run(
                            () =>
                            {
                                try
                                {
                                    m.Invoke(plugin, new object[] { });
                                }
                                catch (Exception e)
                                {
                                    Logger.Write(Logger.Status.Error, e.ToString());
                                }
                            });
                        Logger.Write(Logger.Status.Info, $"Invoked {type.Assembly.FullName}'s OnLoad! ");
                    }
                }

            }
        }

  


        public Assembly GetAssembly(IRequestifyPlugin plugin)
        {
            return plugin.GetType().Assembly;
        }

        public List<Plugin> GetPlugins()
        {
            return Plugins;
        }

        public Plugin GetPlugin(Assembly assembly)
        {
            foreach (var p in Plugins)
            {
                if (p.plugin != null && p.plugin.GetType().Assembly == assembly)
                {
                    return p;
                }
            }

            return null;
        }

        public void DisablePlugin(Plugin plg)
        {
            plg.Status = Status.Disabled;
        }

        public void EnablePlugin(Plugin plg)
        {
            plg.Status = Status.Enabled;
        }

        public Plugin GetPluginFromCommand(CommandManager.RequestifyCommand command)
        {
            return GetPlugin(command.Father);
        }

        public Plugin GetPlugin(string name)
        {
            return Plugins.FirstOrDefault(p => p != null && p.plugin.Name == name);
        }
    }
}