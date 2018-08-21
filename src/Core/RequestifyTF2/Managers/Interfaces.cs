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
using RequestifyTF2.API.Permission;

namespace RequestifyTF2.API
{
    public interface IRequestifyPlugin
    {
        string Author { get; }
        string Name { get; }
        string Desc { get; }
    }

    public interface IRequestifyCommand
    {
        Rules Permission { get; }
        string Help { get; }
        string Name { get; }
        List<string> Alias { get; }
        void Execute(User executor, List<string> arguments);
    }
}