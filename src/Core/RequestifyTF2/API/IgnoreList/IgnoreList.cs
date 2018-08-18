using System.Collections.Generic;

namespace RequestifyTF2.API.IgnoreList
{
   public static class IgnoreList
    {
        private static readonly List<string> _list = new List<string>();
        public static bool Reversed { get; set; }
        public static bool Contains(string name)
        {
            
            var a= _list.Contains(name);
            Logger.Write(Logger.LogStatus.Debug, $"IgnoreList. Contains {name}. Result = {a}");
            return a;
        }

        public static void Add(string name)
        {
         
            if (!_list.Contains(name))
            {
                Logger.Write(Logger.LogStatus.Debug, $"IgnoreList. Adding {name}. Result = true");
                _list.Add(name); 
            }
            else
            {
                Logger.Write(Logger.LogStatus.Debug, $"IgnoreList. Adding {name}. Result = false");
            }
        }

        public static void Remove(string name)
        {
            if (_list.Contains(name))
            {
                Logger.Write(Logger.LogStatus.Debug, $"IgnoreList. Removing {name}. Result = true");
                _list.Remove(name);
            }
            else
            {
                Logger.Write(Logger.LogStatus.Debug, $"IgnoreList. Removing {name}. Result = false");

            }
        }

        public static List<string> GetList => _list;
    }
}
