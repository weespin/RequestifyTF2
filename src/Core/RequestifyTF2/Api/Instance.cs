using System.IO;
using System.Windows.Forms;
using RequestifyTF2.Managers;
using RequestifyTF2.PluginLoader;

namespace RequestifyTF2.Api
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using CSCore;
    using CSCore.CoreAudioAPI;
    using CSCore.SoundOut;

    public static class Instance
    {
       

        /// <summary>
        ///     Stores all Windows audio devices.
        /// </summary>
        public static List<MMDevice> Devices = new List<MMDevice>();


        public static ConcurrentQueue<IWaveSource> QueueForeGround = new ConcurrentQueue<IWaveSource>();

        /// <summary>
        ///     Background channel. Good for long sounds and music.
        /// </summary>
        public static WasapiOut SoundOutBackground = new WasapiOut();
        public static PluginManager Plugins = new PluginManager();
        public static CommandManager Commands ;
        /// <summary>
        ///     Extra channel. Use it for very fast sounds. Does not have a queue!
        /// </summary>
        public static WasapiOut SoundOutExtra = new WasapiOut();

        /// <summary>
        ///     Foreground channel. When sound is playing from this channel, the background channel will be silent.
        /// </summary>
        public static WasapiOut SoundOutForeground = new WasapiOut();

        /// <summary>
        ///     Patching autoexec.cfg, setting audio devices
        /// </summary>
        public static bool Load()
        {
           
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                using (var deviceCollection = deviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    foreach (var device in deviceCollection)
                    {
                        Devices.Add(device);
                    }
                }
            }
            Commands = new CommandManager();
            Logger.Write(Logger.Status.Info, "Patching autoexec.cfg");
            AutoexecChecker.Check();
            Logger.Write(Logger.Status.Info, "Searching for usable devices...");
            MMDevice used = null;
            foreach (var n in Devices)
            {
                if (n.FriendlyName.Contains("Cable") && n.FriendlyName.Contains("Virtual")
                                                     && n.FriendlyName.Contains("Audio"))
                {
                    used = n;
                    break;
                }
            }

            if (used == null)
            {
                Logger.Write(
                    Logger.Status.Error,
                    "Cannot find usable device. Make sure that you have VB-Audio or Virtual Audio Cable installed and enabled!",
                    ConsoleColor.Red);
                return false;
            }

            Logger.Write(Logger.Status.Info, $"Requestify will use {used.FriendlyName}!");
            Logger.Write(
                Logger.Status.Info,
                $"Please set {used.FriendlyName} as default and communication default device in Windows Audio devices -> Recording tab!");
            SoundOutForeground.Device = SoundOutBackground.Device = SoundOutExtra.Device = used;

            return true;
        }

        public static class BackGroundQueue
        {
            public static ConcurrentQueue<Song> PlayList = new ConcurrentQueue<Song>();

            public static void AddSong(Song song)
            {
                PlayList.Enqueue(song);
            }

            public static Song GetFarSong()
            {
                return PlayList.Last();
            }

            /// <summary>
            ///     Don't use it for playing music. It will show nearest songs only.
            /// </summary>
            /// <returns>
            ///     Return Nearest in Queue song. <see cref="Song" />.
            /// </returns>
            public static Song GetNearestSong()
            {
                return PlayList.First();
            }

            public static int GetQueueLenght()
            {
                return PlayList.Count;
            }
        }

        /// <summary>
        ///     Configuration class.
        /// </summary>
        public class Config
        {
            public static string Admin;

            public static string Chiper = new CodeGenerator().GenerateWord(4);

            public static string GameDir;

            public static List<string> Ignored = new List<string>();

            public static bool IgnoredReversed;

            public static bool OnlyWithCode = false;
        }

        public class Song
        {
            public string RequestedBy;

            public IWaveSource Source;

            public string Title;

            public Song(string title, IWaveSource source, string executor)
            {
                this.Title = title;
                this.Source = source;
                this.RequestedBy = executor;
            }
        }
    }
}