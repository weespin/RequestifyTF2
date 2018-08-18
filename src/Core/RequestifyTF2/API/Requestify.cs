using System.Globalization;
using System.Reflection;
using System.Threading;
using RequestifyTF2.Utils;

namespace RequestifyTF2.API
{
    public static class Requestify
    {
        public static RequestifyConsoleHook _writer = new RequestifyConsoleHook();
        static Requestify()
        {
            System.Console.SetOut(_writer);
            System.Console.SetError(_writer);
            Logger.Write(Logger.LogStatus.Info, "=========REQUESTIFYTF2 STARTED========");
            Logger.Write(Logger.LogStatus.Info, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

        }
        public static bool Debug { get; set; }
        public static bool IsMuted { get; set; }
        private static string _gameDir;
        public static string Admin { get; set; }

        public static string GameDir
        {
            get => _gameDir;
            set
            {
                _gameDir = value;
                Logger.Write(Logger.LogStatus.Info, Localization.Localization.CORE_PATCHING_AUTOEXEC);
                Patcher.PatchAutoExec();
            }
        }
        private static ELanguage _language = ELanguage.EN;
        public static CultureInfo GetCulture => Thread.CurrentThread.CurrentUICulture;
        public static ELanguage Language
        {
            get => _language;

            set
            {
                _language = value;
                Thread.CurrentThread.CurrentUICulture = LocalHelper.GetCoreLocalization();
            }
        }
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