using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using RequestifyTF2.Api;

namespace RequestifyTF2.PluginLoader
{
    public static class PluginLoader
    {
        public static ICollection<IRequestifyPlugin> LoadPlugins(string path)
        {
            if (Directory.Exists(path))
            {
                var dllFileNames = Directory.GetFiles(path, "*.dll");

                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach (var dllFile in dllFileNames)
                {
                    var an = AssemblyName.GetAssemblyName(dllFile);
                    var assembly = Assembly.Load(an);
                    assemblies.Add(assembly);
                }

                var pluginType = typeof(IRequestifyPlugin);
                ICollection<Type> pluginTypes = new List<Type>();
                foreach (var assembly in assemblies)
                    if (assembly != null)
                    {
                        var types = assembly.GetTypes();

                        foreach (var type in types)
                            if (type.IsInterface || type.IsAbstract)
                            {
                            }
                            else
                            {
                                if (type.GetInterface(pluginType.FullName) != null)
                                    pluginTypes.Add(type);
                            }
                    }

                ICollection<IRequestifyPlugin> plugins = new List<IRequestifyPlugin>(pluginTypes.Count);
                foreach (var type in pluginTypes)
                {
                    var plugin = (IRequestifyPlugin) Activator.CreateInstance(type);
                    plugins.Add(plugin);
                }

                return plugins;
            }

            return null;
        }
    }
}