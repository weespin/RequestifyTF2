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

using System;
using System.Collections.Generic;
using System.Linq;

namespace RequestifyTF2.API.Permission
{
    [Flags]
    public enum Rules
    {
        None = 0,
        Execute = 1 << 0,
        Equeue = 1 << 1,
        Skip = 1 << 2,
        Assign = 1 << 3,
        All = 1 << 4
    }

    public enum Group
    {
        None,
        User = Rules.Execute | Rules.Equeue,
        DJ = User | Rules.Skip,
        Moderator = DJ | Rules.Assign,
        Admin = Moderator | Rules.All
    }

    public static class Permissions
    {
        private static Dictionary<string, Group> _users = new Dictionary<string, Group>();

        public static bool Exists(string name)
        {
            return _users.ContainsKey(name);
        }

        public static bool Add(string str, Group gr)
        {
            if (!_users.ContainsKey(str))
            {
                _users.Add(str, gr);
                return true;
            }

            return false;
        }

        public static Group GetGroup(string name)
        {
            return _users[name];
        }

        public static bool Modify(string name, Group gr)
        {
            if (_users.ContainsKey(name))
            {
                _users[name] = gr;
                return true;
            }

            return false;
        }

        public static bool RnkUp(string name)
        {
            if (_users.ContainsKey(name))
            {
                if (_users[name] != Group.Admin)
                {
                    _users[name]++;
                    return true;
                }
            }
            else
            {
                _users.Add(name, Group.DJ);
                return true;
            }

            return false;
        }

        public static bool RnkDn(string name)
        {
            if (_users.ContainsKey(name))
            {
                if (_users[name] != Group.None)
                {
                    _users[name]--;
                    return true;
                }
            }
            else
            {
                _users.Add(name, Group.None);
                return true;
            }

            return false;
        }

        public static bool Remove(string name)
        {
            if (_users.ContainsKey(name))
            {
                _users.Remove(name);
                return true;
            }

            return false;
        }

        public static List<Tuple<string, Group>> AllUsers()
        {
            return _users
                .Select(kv => Tuple.Create(kv.Key, kv.Value))
                .ToList();
        }
    }
}