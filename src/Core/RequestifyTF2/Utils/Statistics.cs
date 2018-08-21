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
namespace RequestifyTF2.Utils
{
    public static class Statisctics
    {
        public static uint LinesParsed { get; set; }
        public static uint CommandsParsed { get; set; }
        public static uint GameKills { get; set; }
        public static uint CritsKill { get; set; }
        public static uint Deaths { get; set; }
        public static uint YourDeaths { get; set; }
        public static uint YourKills { get; set; }
        public static uint YourCritsKill { get; set; }
        public static uint ConnectedPlayers { get; set; }
        public static uint Suicides { get; set; }
        public static uint YourSuicides { get; set; }
        public static uint IgnoreListStopped { get; set; }
    }
}