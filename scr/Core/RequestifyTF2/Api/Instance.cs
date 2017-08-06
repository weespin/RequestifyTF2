using System;
using System.Collections.Generic;
using RequestifyTF2.VLC;

namespace RequestifyTF2.Api
{
    public class Instance
    {
        public static List<IRequestifyPlugin> DisabledPlugins = new List<IRequestifyPlugin>();
        public static List<IRequestifyPlugin> ActivePlugins = new List<IRequestifyPlugin>();
        public static VlcRemote Vlc;
   
        public static void Load()
        {
            Logger.Write(Logger.Status.Info, "Loading Instance!");
            AutoexecChecker.Check();
            Vlc = new VlcRemote();
            
            
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

 
}