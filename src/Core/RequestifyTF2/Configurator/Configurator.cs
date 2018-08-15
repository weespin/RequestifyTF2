using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using RequestifyTF2.Api;

namespace RequestifyTF2.Configurator
{
 public static  class Configurator
    {
        public static void GenerateSchema(List<IRequestifyPlugin> plugins)
        {
           
                //LINQ to XML
                XDocument doc = new XDocument(new XElement("pluginconfigs"));

            foreach (var entry in plugins)
            {
                var parsed = GetSchema(entry);
                
                doc.Root.Add(
                    new XElement(, entry.Value.ToString(), new XAttribute("level", entry.Key.ToString())));
            }

            doc.Save(pathName);
            

        }

        public static Dictionary<string, Tuple<Type,object>>[] GetSchema(IRequestifyPlugin plugin)
        {
            /*
             * Array schema
             * [0] = Config
             * [1] = Commands
             */
            Dictionary<string, Tuple<Type, object>>[] rett = new Dictionary<string, Tuple<Type, object>>[2];
            var assembly = Instance.Plugins.GetAssembly(plugin);
            Dictionary<string, Tuple<Type, object>> ret = new Dictionary<string, Tuple<Type, object>>();
            var assamblies = RequestifyTF2.Managers.PluginManager.GetTypesFromInterface(assembly,"IRequestifyConfiguration");
            var asssFieldInfos= assamblies[0].GetFields()
                .ToList();
            foreach (var v in asssFieldInfos)
            {
                object val = null;
                if (v.CustomAttributes.Any())
                {
                    foreach (var s in v.CustomAttributes)
                    {
                        if (s.AttributeType==typeof(DefaultValueAttribute))
                        {
                            if (s.ConstructorArguments[0].ArgumentType == v.FieldType)
                            {
                                val = s.ConstructorArguments[0].Value;
                            }
                        }
                    }
                }
                ret.Add(v.Name,new Tuple<Type, object>(v.FieldType,val));
                
            }
            
            rett[0] = ret;
            var cmnds = Instance.Commands.RegisteredCommand.Where(n => n.Father == assembly);
            var ret2 = new Dictionary<string, Tuple<Type, object>>();
            foreach (var command in cmnds)
            {
                ret2.Add(command.Name,new Tuple<Type,object>(typeof(bool),true));
            }

            rett[1] = ret2;
            return rett;
        }

     
    }
}
