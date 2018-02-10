using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RequestifyTF2.PluginLoader
{
   

    public class Libraries
    {
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
                    // throw new InvalidOperationException(ex);
                }
            }
        }
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
                    Proxy pd = new Proxy();
                    Assembly assemblyz = pd.GetAssembly(assembly);
                    Logger.Write(Logger.Status.Info,$"Loaded {assemblyz.GetName()}" );


                }
                catch (Exception e)
                {
                   Logger.Write(Logger.Status.Error,e.ToString());
                }
            }
           // Assembly.Load();

        }
        public static void LoadFile(string path)
        {

           


       
                try
                {
                    Proxy pd = new Proxy();
                    Assembly assemblyz = pd.GetAssembly(path);
                    Logger.Write(Logger.Status.Info, $"Loaded {assemblyz.GetName()}");


                }
                catch (Exception e)
                {
                    Logger.Write(Logger.Status.Error, e.ToString());
                }
            
            // Assembly.Load();

        }
    }
}
