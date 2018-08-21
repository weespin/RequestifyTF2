// RequestifyTF2
// Copyright (C) 2018  Villiam Nmerukini
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
              Logger.Nlogger.Debug( "RequestifyTF2 Core started "+ Assembly.GetExecutingAssembly().GetName().Version.ToString());
         

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
              Logger.Nlogger.Debug(Localization.Localization.CORE_PATCHING_AUTOEXEC);
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