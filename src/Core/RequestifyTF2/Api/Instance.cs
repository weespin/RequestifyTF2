using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using RequestifyTF2.Managers;
using RequestifyTF2.Utils;

namespace RequestifyTF2.Api
{
    public static class Instance
    {
        /// <summary>
        ///     Stores all Windows audio devices.
        /// </summary>
        public static bool isMuted = false;

        public static ConcurrentQueue<IWaveSource> QueueForeGround = new ConcurrentQueue<IWaveSource>();
        /// <summary>
        ///     Background channel. Good for long sounds and music.
        /// </summary>
        public static WasapiOut SoundOutBackground = new WasapiOut();

        public static PluginManager Plugins = new PluginManager();
        public static CommandManager Commands = new CommandManager();

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
            var GoodInputDevices = new List<MMDevice>();
            var GoodOutputDevices = new List<MMDevice>();
            Logger.Write(Logger.Status.Info, "Patching autoexec.cfg");
            Patcher.PatchAutoExec();
            Logger.Write(Logger.Status.Info, "Searching for usable devices...");
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                using (var deviceoutCollection =
                    deviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    foreach (var device in deviceoutCollection)
                        if (device.FriendlyName.Contains("Cable") && device.FriendlyName.Contains("Virtual")
                                                                  && device.FriendlyName.Contains("Audio"))
                            GoodOutputDevices.Add(device);
                    using (var deviceinpCollection =
                        deviceEnumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active))
                    {
                        foreach (var device in deviceinpCollection) GoodInputDevices.Add(device);
                    }
                }

                if (GoodOutputDevices.Count == 0 || GoodInputDevices.Count == 0)
                {
                    Logger.Write(
                        Logger.Status.Error,
                        "Cannot find usable device. Make sure that you have VB-Audio or Virtual Audio Cable installed and enabled!",
                        ConsoleColor.Red);
                    return false;
                }

                if (GoodOutputDevices.Count(n => n.FriendlyName.Contains("VB-Audio")) != 0 &&
                    GoodInputDevices.Count(n => n.FriendlyName.Contains("VB-Audio")) != 0)
                {
                    var outp = GoodOutputDevices.FirstOrDefault(n => n.FriendlyName.Contains("VB-Audio"));
                    var inp = GoodInputDevices.FirstOrDefault(n => n.FriendlyName.Contains("VB-Audio"));
                    try
                    {
                        AudioDeviceChanger.SetDefaultInputDevice(inp.DeviceID);
                    }
                    catch (Exception e)
                    {
                        Logger.Write(
                            Logger.Status.Error,
                            $"Error while setting {inp.FriendlyName} as default input device\n{e}");
                    }

                    SoundOutForeground.Device = SoundOutBackground.Device = SoundOutExtra.Device = outp;
                    Logger.Write(
                        Logger.Status.STATUS,
                        $"Used {outp.FriendlyName} as output device and {inp.FriendlyName} as input device");
                    return true;
                }

                if (GoodOutputDevices.Count(n => n.FriendlyName.Contains("Virtual Audio Cable")) != 0 &&
                    GoodInputDevices.Count(n => n.FriendlyName.Contains("Virtual Audio Cable")) != 0)
                {
                    var outp = GoodOutputDevices.FirstOrDefault(n =>
                        n.FriendlyName.Contains("Virtual Audio Cable"));
                    var inp = GoodInputDevices.FirstOrDefault(n => n.FriendlyName.Contains("Virtual Audio Cable"));
                    try
                    {
                        AudioDeviceChanger.SetDefaultInputDevice(inp.DeviceID);
                    }
                    catch (Exception e)
                    {
                        Logger.Write(
                            Logger.Status.Error,
                            $"Error while setting {inp.FriendlyName} as default input device\n{e}");
                        return false;
                    }

                    SoundOutForeground.Device = SoundOutBackground.Device = SoundOutExtra.Device = outp;
                    Logger.Write(
                        Logger.Status.STATUS,
                        $"Used {outp.FriendlyName} as output device and {inp.FriendlyName} as input device",
                        ConsoleColor.Red);
                    return true;
                }

                Logger.Write(
                    Logger.Status.Error,
                    "Cannot find usable device. Make sure that you have VB-Audio or Virtual Audio Cable installed and enabled!",
                    ConsoleColor.Red);
                return false;
            }

          
        }
        

        public static string GetDeviceName()
        {
            return SoundOutBackground.Device.DeviceID;
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

            public static string GameDir;

            public static List<string> Ignored = new List<string>();

            public static bool IgnoredReversed;
        }

        public class Song
        {
            public string RequestedBy;

            public IWaveSource Source;

            public string Title;

            public Song(string title, IWaveSource source, string executor)
            {
                Title = title;
                Source = source;
                RequestedBy = executor;
            }
        }
    }
}