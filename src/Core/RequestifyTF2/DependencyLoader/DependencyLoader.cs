using System;
using System.IO;
using System.Reflection;

namespace RequestifyTF2.DependencyLoader
{
    public class DependencyLoader
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
                    Logger.Nlogger.Info(Localization.Localization.CORE_LOADED_PLUGIN, assemblyz.GetName().Name);


                }
                catch (Exception e)
                {
                    Logger.Nlogger.Error(e);
                }
            }
        }

        public static void LoadFile(string path)
        {
            try
            {
                var pd = new Proxy();
                var assemblyz = pd.GetAssembly(path);
                Logger.Nlogger.Info(Localization.Localization.CORE_LOADED_PLUGIN, assemblyz.GetName().Name);
            }
            catch (Exception e)
            {
                Logger.Nlogger.Error(e);
            }
        }

        public class Proxy : MarshalByRefObject
        {
            public Assembly GetAssembly(string assemblyPath)
            {
                try
                {
                    return Assembly.LoadFile(assemblyPath);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}  