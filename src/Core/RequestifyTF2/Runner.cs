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

using System.IO;
using RequestifyTF2.API;
using RequestifyTF2.Threads;

namespace RequestifyTF2
{
    public static class Runner
    {
        public static bool Start()
        {
            if (Requestify.GameDir == string.Empty)
            {
                Logger.Nlogger.Error(Localization.Localization.CORE_SET_DIRECTORY);

                return false;
            }

            if (!Directory.Exists(Requestify.GameDir))
            {
                Logger.Nlogger.Error(Localization.Localization.CORE_ERROR_CANT_FIND_DIR + Requestify.GameDir);

                return false;
            }

            //if (!Instance.Load())
            //{
            //    Logger.Write(
            //        Logger.LogStatus.Error,
            //        Localization.Localization.CORE_ERRORS_FOUND_CLOSE_PROGRAM,
            //        ConsoleColor.Red);
            //    return false;
            //}

            //if (!File.Exists(Instance.GameDir + "/console.log"))
            //{
            //    //try
            //    //{
            //    //    File.WriteAllText(Instance.GameDir + "/console.log", string.Empty);
            //    //}
            //    //catch
            //    //{
            //    //    Logger.Write(
            //    //        Logger.LogStatus.Error,
            //    //        Localization.Localization.CORE_ERROR_CANT_PREPARE_CONSOLE_LOG,
            //    //        ConsoleColor.Red);
            //    //    return false;
            //    //}
            //}


            PlayerThread.StartThread();
            ReaderThread.StartThread();
            return true;
        }
    }
}