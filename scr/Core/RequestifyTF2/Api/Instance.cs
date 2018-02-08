using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;

namespace RequestifyTF2.Api
{
    public static class  Instance
    {
        public static List<MMDevice> _devices = new List<MMDevice>();
        public static List<IRequestifyPlugin> DisabledPlugins = new List<IRequestifyPlugin>();
        public static List<IRequestifyPlugin> ActivePlugins = new List<IRequestifyPlugin>();
        public static ConcurrentQueue<IWaveSource> QueueBackGround = new ConcurrentQueue<IWaveSource>();
        public static ConcurrentQueue<IWaveSource> QueueForeGround = new ConcurrentQueue<IWaveSource>();
        public static WasapiOut SoundOutForeground = new WasapiOut();
        public static WasapiOut SoundOutBackground = new WasapiOut();
        public static WasapiOut SoundOutExtra = new WasapiOut();

        public static void Load()
        {
            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            {
                using (
                    var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    foreach (var device in mmdeviceCollection)
                    {
                        _devices.Add(device);
                    }
                }
            }
            Logger.Write(Logger.Status.Info, "Loading Instance!");
            AutoexecChecker.Check();
            SoundOutForeground.Device = SoundOutBackground.Device = SoundOutExtra.Device =
                _devices.Where(n => n.FriendlyName.Contains("Virtual")).FirstOrDefault();

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