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
                    Logger.Write(Logger.LogStatus.Info, string.Format(Localization.Localization.CORE_LOADED_PLUGIN, assemblyz.GetName()));


                }
                catch (Exception e)
                {
                    Logger.Write(Logger.LogStatus.Error, e.ToString());
                }
            }
        }

        public static void LoadFile(string path)
        {
            try
            {
                var pd = new Proxy();
                var assemblyz = pd.GetAssembly(path);
                Logger.Write(Logger.LogStatus.Info, string.Format(Localization.Localization.CORE_LOADED_PLUGIN, assemblyz.GetName()));
            }
            catch (Exception e)
            {
                Logger.Write(Logger.LogStatus.Error, e.ToString());
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