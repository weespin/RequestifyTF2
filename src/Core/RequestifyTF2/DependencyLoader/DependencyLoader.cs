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
using System.IO;
using System.Reflection;

namespace RequestifyTF2.DependencyLoader
{
    public class DependencyLoader
    {
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
                    var proxy = new Proxy();
                    var assemblyz = proxy.GetAssembly(assembly);
                    Logger.Nlogger.Info(Localization.Localization.CORE_LOADED_PLUGIN, assemblyz.GetName().Name);
                }
                catch (Exception e)
                {
                    Logger.Nlogger.Error(e);
                }
            }
        }

        public static void LoadFile(string path)
        {
            try
            {
                var pd = new Proxy();
                var assemblyz = pd.GetAssembly(path);
                Logger.Nlogger.Info(Localization.Localization.CORE_LOADED_PLUGIN, assemblyz.GetName().Name);
            }
            catch (Exception e)
            {
                Logger.Nlogger.Error(e);
            }
        }

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
                }
            }
        }
    }
}