using System;
using System.Collections.Generic;
using RequestifyTF2.VLC;
using RequestifyTF2.VLCUpdater;

namespace RequestifyTF2.Api
{
    public class Instance
    {
        public static List<IRequestifyPlugin> DisabledPlugins = new List<IRequestifyPlugin>();
        public static List<IRequestifyPlugin> ActivePlugins = new List<IRequestifyPlugin>();
        public static VlcRemote Vlc;
        public static LogDeph LogDeph;
        public static void Load()
        {
            Logger.Write(Logger.Status.Info, "Loading Instance!");
            new AutoexecChecker();
            new Update();
            Vlc = new VlcRemote();
            Fixer.Fix();
            
        }

        public class Config
        {
            public static string Chiper = new CodeGenerator().GenerateWord(4);
            public static List<string> Ignored = new List<string>();
            public static bool IgnoredReversed;
            public static bool OnlyWithCode = false;
            public static string GameDir;
            public static string AhkPath;
        }

      
       
    }

    public enum LogDeph
    {
        None,
        Errors,
        Invoke,
        All
    }
}