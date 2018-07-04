using System;
using System.IO;
using RequestifyTF2.Api;
using RequestifyTF2.Threads;

namespace RequestifyTF2
{
    public static class Runner
    {
        public static bool Start()
        {
            if (Instance.Config.GameDir == string.Empty)
            {
                Logger.Write(Logger.Status.Error, "Please set the game directory");

                return false;
            }

            if (!Directory.Exists(Instance.Config.GameDir))
            {
                Logger.Write(Logger.Status.Error, "Can't find directory " + Instance.Config.GameDir);

                return false;
            }

            if (!Instance.Load())
            {
                Logger.Write(
                    Logger.Status.Error,
                    "Errors found. Please fix errors before using this program.",
                    ConsoleColor.Red);
                return false;
            }

            if (File.Exists(Instance.Config.GameDir + "/console.log"))
                try
                {
                    File.WriteAllText(Instance.Config.GameDir + "/console.log", string.Empty);
                }
                catch
                {
                    Logger.Write(
                        Logger.Status.Error,
                        "Can't remove lines from console.log. The game is probably running. Please close the game before starting this program",
                        ConsoleColor.Red);
                    return false;
                    // return;
                }

            PlayerThread.Starter();
            ReaderThread.Starter();
            return true;
        }
    }
}