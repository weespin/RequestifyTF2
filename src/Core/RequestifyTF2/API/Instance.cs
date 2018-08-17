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
using RequestifyTF2.Audio.Utils;
using RequestifyTF2.Managers;
using RequestifyTF2.Utils;

namespace RequestifyTF2.API
{
    public static class Instance
    {
        /// <summary>
        ///     Stores all Windows audio devices.
        /// </summary>
        public static bool IsMuted { get; set; }
        private static string _gameDir;
        public static string Admin { get; set; }

        public static string GameDir
        {
            get => _gameDir;
            set
            {
                _gameDir = value;
                Logger.Write(Logger.Status.Info, Localization.Localization.CORE_PATCHING_AUTOEXEC);
                Patcher.PatchAutoExec();
            }
        }

        private static ELanguage _language = ELanguage.EN;
        //todo: make this garbage shorter
        public static CultureInfo GetCulture => Thread.CurrentThread.CurrentUICulture;
        public static ELanguage Language
        {
            get => _language;

            set
            {
                _language = value;
                System.Threading.Thread.CurrentThread.CurrentUICulture = LocalHelper.GetCoreLocalization();
            }
        }

      
        /// <summary>
        ///     Patching autoexec.cfg, setting audio devices
        /// </summary>
      
       
        public enum ELanguage
        {
            BG = 0,
            CS = 1,
            DA = 2,
            DE = 3,
            EL = 4,
            ES = 5,
            FI = 6,
            FR = 7,
            HU = 8,
            IT = 9,
            JA = 10,
            KO = 11,
            NL = 12,
            NN = 13,
            PL = 14,
            BR = 15,
            PT = 16,
            EN = 17,
            RO = 18,
            RU = 19,
            SV = 20,
            TH = 21,
            TR = 22,
            UK = 23,
            TZN = 24,
            SZN = 25
        }
    }
}