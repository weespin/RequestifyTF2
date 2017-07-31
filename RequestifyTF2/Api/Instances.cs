using System.Collections.Generic;
using RequestifyTF2.VLC;

namespace RequestifyTF2.Api
{
    public class Instances
    {
        public static List<IPlugin> DisabledPlugins = new List<IPlugin>();
        public static List<IPlugin> ActivePlugins = new List<IPlugin>();
        public static VlcRemote Vlc;

        public static void Load()
        {
            Vlc = new VlcRemote();
            Fixer.Fix();
        }

        public class Config
        {
            public static string Chiper = new CodeGenerator().GenerateWord(4);
            public static List<string> Ignored = new List<string>();
            public static bool IgnoredReversed;
            public static bool OnlyAdmin = false;
            public static string GameDir;
            public static string AhkPath;
        }
    }
}