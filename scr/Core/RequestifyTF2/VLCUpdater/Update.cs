using System;
using System.IO;
using System.Net;
using Microsoft.Win32;

namespace RequestifyTF2.VLCUpdater
{
    internal class Update
    {
        public Update()
        {
            Logger.Write(Logger.Status.STATUS, "Updating VLC Lua files");
            //Its 2:42AM, 13.12.2016. Happy NEW Year Weespin. P.S. Listening https://goreshit.bandcamp.com/
            string vlcPath = null;

            var vlcKey = Registry.LocalMachine.OpenSubKey(@"Software\VideoLan\VLC");

            if (vlcKey == null)
                vlcKey = Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\VideoLan\VLC");

            if (vlcKey != null)
                vlcPath = vlcKey.GetValue(null) as string;


            if (vlcPath == null)
            {
                Logger.Write(Logger.Status.Error, "U didnt installer VLC Player", ConsoleColor.Red);
                Logger.Write(Logger.Status.Error, "PRESS ANY KEY TO QUIT", ConsoleColor.Red);
                Console.Read();
                Environment.Exit(0);
            }
            var installdir = vlcKey.GetValue("InstallDir");
            var plugindir = installdir + @"\lua\playlist\";
            if (File.Exists(plugindir + "youtube.luac"))
                try
                {
                    using (var web = new WebClient())
                    {
                        web.Proxy = null;
                        File.WriteAllText(plugindir + "youtube.luac",
                            web.DownloadString(
                                "https://raw.githubusercontent.com/videolan/vlc/master/share/lua/playlist/youtube.lua"));
                    }
                }
                catch (Exception)
                {
                    Logger.Write(Logger.Status.Error,
                        "Cant update youtube.luac \n Run this programm as Administrator",
                        ConsoleColor.Red);
                }
            if (File.Exists(plugindir + "soundcloud.luac"))
                try
                {
                    using (var web = new WebClient())
                    {
                        web.Proxy = null;
                        File.WriteAllText(plugindir + "soundcloud.luac",
                            web.DownloadString(
                                "https://raw.githubusercontent.com/videolan/vlc/master/share/lua/playlist/soundcloud.lua"));
                    }
                }
                catch (Exception)
                {
                    Logger.Write(Logger.Status.Error,
                        "Cant update soundcloud.luac \n Run this programm as Administrator",
                        ConsoleColor.Red);
                }
        }
    }
}