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

        private static ELanguage _language;
        //todo: make this garbage shorter
        public static ELanguage Language
        {
            get
            {
                return _language;
            }

            set
            {
                switch (value)
                {
                    case ELanguage.BG:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("bg");
                        break;
                    case ELanguage.CS:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("cs");
                        break;
                    case ELanguage.DA:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("da");
                        break;
                    case ELanguage.DE:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de");
                        break;
                    case ELanguage.EL:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("el");
                        break;
                    case ELanguage.ES:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es");
                        break;
                    case ELanguage.FI:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fi");
                        break;
                    case ELanguage.FR:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr");
                        break;
                    case ELanguage.HU:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("hu");
                        break;
                    case ELanguage.IT:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("it");
                        break;
                    case ELanguage.JA:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ja");
                        break;
                    case ELanguage.KO:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ko");
                        break;
                    case ELanguage.NL:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("nl");
                        break;
                    case ELanguage.NN:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("nn");
                        break;
                    case ELanguage.PL:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pl");
                        break;
                    case ELanguage.BR:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
                        break;
                    case ELanguage.PT:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt");
                        break;
                    case ELanguage.EN:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                        break;
                    case ELanguage.RO:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ro");
                        break;
                    case ELanguage.RU:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru");
                        break;
                    case ELanguage.SV:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("sv");
                        break;
                    case ELanguage.TH:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("th");
                        break;
                    case ELanguage.TR:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("tr");
                        break;
                    case ELanguage.UK:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("uk");
                        break;
                    case ELanguage.TZN:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh");
                        break;
                    case ELanguage.SZN:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
                _language = value;
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

                    SoundOutForeground.Device = SoundOutBackground.Device = SoundOutExtra.Device = outp;
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
            public User RequestedBy;

            public IWaveSource Source;

            public string Title;

            public Song(string title, IWaveSource source, User executor)
            {
                Title = title;
                Source = source;
                RequestedBy = executor;
            }
        }
    }
}