using System;
using System.Collections.Generic;
using System.IO;

using RequestifyTF2.Api;
using RequestifyTF2.VLC;


namespace RequestifyTF2
{
    public static class Runner
    {
        public static List<string> Ignored = new List<string>();
        public static VlcRemote Vlc = Instance.Vlc;
        public static Instance.Config Cfg = new Instance.Config();
        public static bool IsGameRunning;

        public static void Start()
        {
           
            if (Instance.Config.GameDir == "")
            {
             Console.WriteLine("Please set the game directory");

                return;

            }
            Instance.Load();

            if (File.Exists(Instance.Config.GameDir + "/console.log"))
                try
                {
                    File.WriteAllText(Instance.Config.GameDir + "/console.log", "");
                }
                catch 
                {
                    // ignored
                }

            
            ReaderThread.Starter();
            


            //Init
        }
    }
}