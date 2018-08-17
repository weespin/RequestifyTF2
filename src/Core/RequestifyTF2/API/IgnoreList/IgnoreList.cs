using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestifyTF2.API.IgnoreList
{
   public static class IgnoreList
    {
        private static readonly List<string> _list = new List<string>();
        public static bool Reversed { get; set; }
        public static bool Contains(string name)
        {
            return _list.Contains(name);
        }

        public static void Add(string name)
        {
            if (!_list.Contains(name))
            {
                _list.Add(name);
            }
        }

        public static void Remove(string name)
        {
            if (_list.Contains(name))
            {
                _list.Remove(name);
            }
        }

        public static List<string> GetList => _list;
    }
}
