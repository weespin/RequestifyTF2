using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using CSCore;
using CSCore.Codecs.AAC;
using CSCore.Codecs.MP3;
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
        public static bool IsMuted { get; set; } = false;

        public static config Config = new config();
        public static ConcurrentQueue<IWaveSource> QueueForeGround { get; set; } = new ConcurrentQueue<IWaveSource>();
        /// <summary>
        ///     Background channel. Good for long sounds and music.
        /// </summary>
        public static WasapiOut SoundOutBackground { get; set; } = new WasapiOut();

        public static PluginManager Plugins { get; set; } = new PluginManager();
        public static CommandManager Commands { get; set; } = new CommandManager();

        /// <summary>
        ///     Extra channel. Use it for very fast sounds. Does not have a queue!
        /// </summary>
        public static WasapiOut SoundOutExtra { get; set; } = new WasapiOut();

        /// <summary>
        ///     Foreground channel. When sound is playing from this channel, the background channel will be silent.
        /// </summary>
        public static WasapiOut SoundOutForeground { get; set; } = new WasapiOut();

        private static ELanguage _language = ELanguage.EN;
        //todo: make this garbage shorter
        public static CultureInfo GetCulture => Thread.CurrentThread.CurrentUICulture;
        public static ELanguage Language
        {
            get
            {
                return _language;
            }

            set
            {
                _language = value;
                System.Threading.Thread.CurrentThread.CurrentUICulture = LocalHelper.GetCoreLocalization();
            }
        }

        public enum SongType
        {
            MP3,
            AAC
        }
        public static bool AddEqueue(SongType songtype, string Link, string RequestedBy,string title)
        {

            try
            {

         
            if(songtype == SongType.MP3)
            { 
            BackGroundQueue.PlayList.Enqueue(new Song(title, new Mp3MediafoundationDecoder(Link),new User(){Name = RequestedBy,Tag=0}));
            }
            else
            {
                BackGroundQueue.PlayList.Enqueue(new Song(title, new AacDecoder(Link), new User() { Name = RequestedBy, Tag = 0 }));
            }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        ///     Patching autoexec.cfg, setting audio devices
        /// </summary>
        public static bool Load()
        {
            var GoodInputDevices = new List<MMDevice>();
            var GoodOutputDevices = new List<MMDevice>();
            Logger.Write(Logger.Status.Info, Localization.Localization.CORE_PATCHING_AUTOEXEC);
            Patcher.PatchAutoExec();
            Logger.Write(Logger.Status.Info, Localization.Localization.CORE_SEARCHING_FOR_DEVICES);
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                using (var deviceoutCollection =
                    deviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    foreach (var device in deviceoutCollection)
                    {
                        if (device.FriendlyName.Contains("Cable") && device.FriendlyName.Contains("Virtual")
                                                                  && device.FriendlyName.Contains("Audio"))
                        {
                            GoodOutputDevices.Add(device);
                        }
                    }

                    using (var deviceinpCollection =
                        deviceEnumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active))
                    {
                        foreach (var device in deviceinpCollection)
                        {
                            GoodInputDevices.Add(device);
                        }
                    }
                }

                if (GoodOutputDevices.Count == 0 || GoodInputDevices.Count == 0)
                {
                    Logger.Write(
                        Logger.Status.Error,
                        Localization.Localization.CORE_CANNOT_FIND_DEVICES,
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
                            string.Format(Localization.Localization.CORE_ERROR_WHILE_SETTING_INPUT, inp.FriendlyName, e));
                    }

                    SoundOutExtra.Device = outp;
                    SoundOutBackground.Device = outp;
                    SoundOutForeground.Device = outp;
                    Logger.Write(
                        Logger.Status.STATUS,
                        string.Format(Localization.Localization.CORE_USED_DEVICES, outp.FriendlyName, inp.FriendlyName));
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
                            string.Format(Localization.Localization.CORE_ERROR_WHILE_SETTING_INPUT, inp.FriendlyName, e));
                        return false;
                    }

                    SoundOutForeground.Device = SoundOutBackground.Device = SoundOutExtra.Device = outp;
                    Logger.Write(
                        Logger.Status.STATUS,
                        string.Format(Localization.Localization.CORE_USED_DEVICES, outp.FriendlyName, inp.FriendlyName),
                        ConsoleColor.Red);
                    return true;
                }

                Logger.Write(
                    Logger.Status.Error,
                    Localization.Localization.CORE_CANNOT_FIND_DEVICES,
                    ConsoleColor.Red);
                return false;
            }

          
        }

       
        public enum ELanguage
        {
            BG,
            CS,
            DA,
            DE,
            EL,
            ES,
            FI,
            FR,
            HU,
            IT,
            JA,
            KO,
            NL,
            NN,
            PL,
            BR,
            PT,
            EN,
            RO,
            RU,
            SV,
            TH,
            TR,
            UK,
            TZN,
            SZN
        }
        public static string GetDeviceName()
        {
            return SoundOutBackground.Device.DeviceID;
        }

        public static class BackGroundQueue
        {
            public static ConcurrentQueue<Song> PlayList { get; set; } = new ConcurrentQueue<Song>();

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
        public class config
        {
            public string Admin { get; set; }

            public string GameDir { get; set; }

            public List<string> Ignored { get; set; } = new List<string>();

            public bool IgnoredReversed { get; set; }
        }

        public class Song
        {
            public bool Dequeued { get; set; }

            public User RequestedBy { get; set; }

            public IWaveSource Source { get; set; }

            public string Title { get; set; }

            public Song(string title, IWaveSource source, User executor)
            {
                Title = title;
                Source = source;
                RequestedBy = executor;
            }
        }
    }
}