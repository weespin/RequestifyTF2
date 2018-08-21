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
            Logger.Nlogger.Debug($"IgnoreList. Contains {name}. Result = {a}");
            return a;
        }

        public static void Add(string name)
        {
         
            if (!_list.Contains(name))
            {
                Logger.Nlogger.Debug($"IgnoreList. Adding {name}.");
                _list.Add(name); 
            }
        }

        public static void Remove(string name)
        {
            if (_list.Contains(name))
            {
                Logger.Nlogger.Debug($"IgnoreList. Removing {name}.");
                _list.Remove(name);
            }
           
        }

        public static List<string> GetList => _list;
    }
}
