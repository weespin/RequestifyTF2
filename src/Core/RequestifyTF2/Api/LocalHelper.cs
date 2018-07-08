using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequestifyTF2.Api;

namespace RequestifyTF2.Api
{
   public static class LocalHelper
    {
        public static CultureInfo GetCoreLocalization()
        {
            switch (Instance.Language)
            {

                case Instance.ELanguage.BG:
                    return new CultureInfo("bg");
                case Instance.ELanguage.CS:
                    return new CultureInfo("cs");
                case Instance.ELanguage.DA:
                    return new CultureInfo("da");
                case Instance.ELanguage.DE:
                    return new CultureInfo("de");
                case Instance.ELanguage.EL:
                    return new CultureInfo("el");
                case Instance.ELanguage.ES:
                    return new CultureInfo("es");
                case Instance.ELanguage.FI:
                    return new CultureInfo("fi");
                case Instance.ELanguage.FR:
                    return new CultureInfo("fr");
                case Instance.ELanguage.HU:
                    return new CultureInfo("hu");
                case Instance.ELanguage.IT:
                    return new CultureInfo("it");
                case Instance.ELanguage.JA:
                    return new CultureInfo("ja");
                case Instance.ELanguage.KO:
                    return new CultureInfo("ko");
                case Instance.ELanguage.NL:
                    return new CultureInfo("nl");
                case Instance.ELanguage.NN:
                    return new CultureInfo("nn");
                case Instance.ELanguage.PL:
                    return new CultureInfo("pl");
                case Instance.ELanguage.BR:
                    return new CultureInfo("pt-BR");
                case Instance.ELanguage.PT:
                    return new CultureInfo("pt");
                case Instance.ELanguage.EN:
                    return new CultureInfo("en");
                case Instance.ELanguage.RO:
                    return new CultureInfo("ro");
                case Instance.ELanguage.RU:
                    return new CultureInfo("ru");
                case Instance.ELanguage.SV:
                    return new CultureInfo("sv");
                case Instance.ELanguage.TH:
                    return new CultureInfo("th");
                case Instance.ELanguage.TR:
                    return new CultureInfo("tr");
                case Instance.ELanguage.UK:
                    return new CultureInfo("uk");
                case Instance.ELanguage.TZN:
                    return new CultureInfo("zh");
                case Instance.ELanguage.SZN:
                    return new CultureInfo("zh-CN");
                default:
                    return new CultureInfo("en-US");
            }
        }
    }
}
