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
using RequestifyTF2.API.Permission;

namespace RequestifyTF2.API
{
    [Flags]
    public enum Tag
    {
        None = 0,
        Spectator = 1 << 0,
        Team = 1 << 1,
        Dead = 1 << 2
    }

   

    public class User
    {
        public string Name { get; set; } = "";
        public Tag Tag { get; set; } = Tag.None;
        public Group Group { get; set; } = Group.User;
    }
}