using System;
using System.IO;
using RequestifyTF2.API;
using RequestifyTF2.Threads;

namespace RequestifyTF2
{
    public static class Runner
    {
        public static bool Start()
        {
            if (Instance.Config.GameDir == string.Empty)
            {
                Logger.Write(Logger.Status.Error, Localization.Localization.CORE_SET_DIRECTORY);

                return false;
            }

            if (!Directory.Exists(Instance.Config.GameDir))
            {
                Logger.Write(Logger.Status.Error, Localization.Localization.CORE_ERROR_CANT_FIND_DIR + Instance.Config.GameDir);

                return false; 
            }

            if (!Instance.Load())
            {
                Logger.Write(
                    Logger.Status.Error,
                    Localization.Localization.CORE_ERRORS_FOUND_CLOSE_PROGRAM,
                    ConsoleColor.Red);
                return false;
            }

            if (File.Exists(Instance.Config.GameDir + "/console.log"))
            {
                //try
                //{
                //    File.WriteAllText(Instance.Config.GameDir + "/console.log", string.Empty);
                //}
                //catch
                //{
                //    Logger.Write(
                //        Logger.Status.Error,
                //        Localization.Localization.CORE_ERROR_CANT_PREPARE_CONSOLE_LOG,
                //        ConsoleColor.Red);
                //    return false;
                //}
            }

            PlayerThread.StartThread();
            ReaderThread.StartThread();
            return true;
        }
    }
}