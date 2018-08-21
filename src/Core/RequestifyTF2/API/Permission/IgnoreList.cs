// RequestifyTF2
// Copyright (C) 2018  Villiam Nmerukini
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
