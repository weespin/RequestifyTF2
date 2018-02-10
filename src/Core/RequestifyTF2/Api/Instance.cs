using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RequestifyTF2.Api
{
    public static class  Instance
    {
        /// <summary>
        /// Stores all Windows audio devices.
        /// </summary>
        public static List<MMDevice> Devices = new List<MMDevice>();
        
        public static List<IRequestifyPlugin> DisabledPlugins = new List<IRequestifyPlugin>();
        public static List<IRequestifyPlugin> ActivePlugins = new List<IRequestifyPlugin>();
        public static ConcurrentQueue<IWaveSource> QueueBackGround = new ConcurrentQueue<IWaveSource>();
        public static ConcurrentQueue<IWaveSource> QueueForeGround = new ConcurrentQueue<IWaveSource>();

        /// <summary>
        /// Foreground channel. When sound is playing from this channel, the background channel will be silent.
        /// </summary>
        public static WasapiOut SoundOutForeground = new WasapiOut();

        /// <summary>
        /// Background channel. Good for long sounds and music.
        /// </summary>
        public static WasapiOut SoundOutBackground = new WasapiOut();

        /// <summary>
        /// Extra channel. Use it for very fast sounds. Does not have a queue!
        /// </summary>
        public static WasapiOut SoundOutExtra = new WasapiOut();

        /// <summary>
        /// Patching autoexec.cfg, setting audio devices
        /// </summary>
        public static bool Load()
        {
            
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                using (
                    var deviceCollection = deviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                   {
                    foreach (var device in deviceCollection)
                    {
                        Devices.Add(device);
                    }
                }
            }
            Logger.Write(Logger.Status.Info, "Patching autoexec.cfg");
            AutoexecChecker.Check();
            Logger.Write(Logger.Status.Info, "Searching for usable devices...");
            MMDevice used = null;
            foreach (var n in Devices)
            {
                if (n.FriendlyName.Contains("Cable") && n.FriendlyName.Contains("Virtual") && n.FriendlyName.Contains("Audio"))
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
            Logger.Write(Logger.Status.Info,$"Please set {used.FriendlyName} as default and communication default device in Windows Audio devices -> Recording tab!");
            SoundOutForeground.Device = SoundOutBackground.Device = SoundOutExtra.Device = used;
            return true;
        }

        /// <summary>
        /// Configuration class.
        /// </summary>
        public class Config
        {
          
            public static string Chiper = new CodeGenerator().GenerateWord(4);
            public static List<string> Ignored = new List<string>();
            public static bool IgnoredReversed;
            public static bool OnlyWithCode = false;
            public static string GameDir;
            public static string Admin;
        }
    }

 
}