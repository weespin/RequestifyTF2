using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using RequestifyTF2.Api;
using RequestifyTF2.PluginLoader;

namespace RequestifyTF2.Managers
{
    public class PluginManager
    {
        public enum Status
        {
            Enabled,
            Disabled
        }

        private static readonly List<Type> plugintypes = new List<Type>();
        public static readonly List<Assembly> pluginAssemblies = new List<Assembly>();
        private static readonly List<Plugin> Plugins = new List<Plugin>();

        public PluginManager()
        {
            if (!Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/"))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/");
            }

         
            loadPlugins(
                Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/");
        }

        public static Dictionary<string, string> FindAllPlugins(string directory, string extension = "*.dll")
        {
            var dict = new Dictionary<string, string>();
            IEnumerable<FileInfo> dlls = new DirectoryInfo(directory).GetFiles(extension, SearchOption.AllDirectories);
            foreach (var file in dlls)
            {
                try
                {
                    var name = AssemblyName.GetAssemblyName(file.FullName);
                    dict.Add(name.FullName, file.FullName);
                }
                catch (Exception e)
                {
                    Logger.Write(Logger.Status.Error,e.ToString());
                }
            }

            return dict;
        }

        public static List<Assembly> LoadAssembliesFromDirectory(string directory, string extension = "*.dll")
        {
            var assemblies = new List<Assembly>();
            IEnumerable<FileInfo> pluginsLibraries =
                new DirectoryInfo(directory).GetFiles(extension, SearchOption.AllDirectories);

            foreach (var library in pluginsLibraries)
            {
                try
                {
                    var assembly = Assembly.LoadFile(library.FullName);

                    var types = GetTypesFromInterface(assembly, "IRequestifyPlugin");

                    if (types.Count == 1)
                    {
                        Console.WriteLine(Localization.Localization.CORE_PLUGIN_LOADING_FROM, assembly.GetName().Name, assembly.Location);
                        assemblies.Add(assembly);
                        Plugins.Add(new Plugin(Activator.CreateInstance(types[0]) as IRequestifyPlugin,
                            Status.Enabled));
                    }
                    else if (types.Count > 1)
                    {
                        Console.WriteLine(Localization.Localization.CORE_INVALID_PLUGIN_MORE_THAN_ONE_INTERFACE, assembly.GetName().Name);
                    }
                    else

                    {
                        Console.WriteLine(Localization.Localization.CORE_INVALID_PLUGIN + assembly.GetName().Name);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Localization.Localization.CORE_CANT_LOAD_PLUGIN + library.Name);
                    Console.WriteLine(ex);
                }
            }

            return assemblies;
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

        public void loadPlugins(string path)
        {
            Libraries.Load(Path.GetDirectoryName(Application.ExecutablePath) + "/lib/");
            var libraries = FindAllPlugins(path);
           

            foreach (var pair in libraries)
            {
                if (!libraries.ContainsKey(pair.Key))
                {
                    libraries.Add(pair.Key, pair.Value);
                }
            }

            var asseblylist = LoadAssembliesFromDirectory(path);
            foreach (var assebly in asseblylist)
            {
                pluginAssemblies.Add(assebly);
            }
            var pluginImplemenations = GetTypesFromInterface(pluginAssemblies, "IRequestifyPlugin");
            foreach (var pluginType in pluginImplemenations)
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
                        
                                try
                                {
                                    m.Invoke(plugin, new object[] { });
                                }
                                catch (Exception e)
                                {
                                    Logger.Write(Logger.Status.Error, e.ToString());
                                }
                          
                        Logger.Write(Logger.Status.Info, string.Format(Localization.Localization.CORE_INVOKED_ONLOAD_METHOD, type.Assembly.FullName));
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

        public class Plugin
        {
            public IRequestifyPlugin plugin;
            public Status Status;

            public Plugin(IRequestifyPlugin Plugin, Status status)
            {
                plugin = Plugin;
                Status = status;
            }
        }
    }
}