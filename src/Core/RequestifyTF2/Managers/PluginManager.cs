using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using RequestifyTF2.API;
using RequestifyTF2.API.Events;
using RequestifyTF2.DependencyLoader;

namespace RequestifyTF2.Managers
{
    public static class PluginManager
    {
        public enum Status
        {
            Enabled,
            Disabled
        }


        public static readonly List<Plugin> Plugins = new List<Plugin>();
        static PluginManager()
        {
            CommandManager.Init();
            if (!Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/"))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/");
            }
            if (!Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/lib/"))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "/lib/");
            }
            LoadLibs(Path.GetDirectoryName(Application.ExecutablePath) + "/lib/");
            LoadPluginsFromDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "/plugins/");
        }

        public static Dictionary<string, string> FindAllAssembly(string directory, string extension = "*.dll")
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
                    Logger.Write(Logger.LogStatus.Error,e.ToString());
                }
            }

            return dict;
        }

        public static void LoadPluginsFromDirectory(string directory, string extension = "*.dll")
        {
         
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
                        Plugins.Add(new Plugin(Activator.CreateInstance(types[0]) as IRequestifyPlugin,
                            Status.Enabled));
                        var onload = assembly.GetTypes();
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
                                    Logger.Write(Logger.LogStatus.Error, e.ToString());
                                }

                                Logger.Write(Logger.LogStatus.Info, string.Format(Localization.Localization.CORE_INVOKED_ONLOAD_METHOD, type.Assembly.FullName));
                            }
                        }
                        Events.PluginLoaded.Invoke(GetPlugin(assembly).plugin);
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

       
        public static void LoadLibs(string path)
        {
            DependencyLoader.DependencyLoader.Load(path);
            var libraries = FindAllAssembly(path);
            foreach (var pair in libraries)
            {
                if (!libraries.ContainsKey(pair.Key))
                {
                    libraries.Add(pair.Key, pair.Value);
                }
            }

        }
   
        public static Assembly GetAssembly(this IRequestifyPlugin plugin)
        {
            return plugin.GetType().Assembly;
        }
        public static List<Plugin> GetPlugins()
        {
            return Plugins;
        }
        public static Plugin GetPlugin(Assembly assembly)
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
        public static void Disable(this Plugin plg)
        {
            plg.Status = Status.Disabled;
        }

        public static void Enable(this Plugin plg)
        {
            plg.Status = Status.Enabled;
        }

        public static Plugin GetPluginFromCommand(CommandManager.RequestifyCommand command)
        {
            return GetPlugin(command.Father);
        }

        public static Plugin GetPlugin(string name)
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