﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace RequestifyTF2.PluginLoader
{
    public class Libraries
    {
        public static void Load(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var dllFileNames = Directory.GetFiles(path, "*.dll");
            foreach (var assembly in dllFileNames)
            {
                try
                {
                    var proxy = new Proxy();
                    var assemblyz = proxy.GetAssembly(assembly);
                    Logger.Write(Logger.Status.Info, string.Format(Localization.Localization.CORE_LOADED_PLUGIN, assemblyz.GetName()));


                }
                catch (Exception e)
                {
                    Logger.Write(Logger.Status.Error, e.ToString());
                }
            }
        }

        public static bool LoadFile(string path)
        {
            try
            {
                var pd = new Proxy();
                var assembly = pd.GetAssembly(path);
                Logger.Write(Logger.Status.Info, string.Format(Localization.Localization.CORE_LOADED_PLUGIN, assembly.GetName()));
                return true;
            }
            catch (Exception e)
            {
                Logger.Write(Logger.Status.Error, e.ToString());
            }

            return false;
        }

        public class Proxy : MarshalByRefObject
        {
            public Assembly GetAssembly(string assemblyPath)
            {
                try
                {
	                return Assembly.LoadFrom(assemblyPath);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
    }
}  