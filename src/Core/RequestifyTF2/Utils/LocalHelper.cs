using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequestifyTF2.API;

namespace RequestifyTF2.API
{
   public static class LocalHelper
    {
        public static CultureInfo GetCoreLocalization()
        {
            switch (Requestify.Language)
            {

                case Requestify.ELanguage.BG:
                    return new CultureInfo("bg");
                case Requestify.ELanguage.CS:
                    return new CultureInfo("cs");
                case Requestify.ELanguage.DA:
                    return new CultureInfo("da");
                case Requestify.ELanguage.DE:
                    return new CultureInfo("de");
                case Requestify.ELanguage.EL:
                    return new CultureInfo("el");
                case Requestify.ELanguage.ES:
                    return new CultureInfo("es");
                case Requestify.ELanguage.FI:
                    return new CultureInfo("fi");
                case Requestify.ELanguage.FR:
                    return new CultureInfo("fr");
                case Requestify.ELanguage.HU:
                    return new CultureInfo("hu");
                case Requestify.ELanguage.IT:
                    return new CultureInfo("it");
                case Requestify.ELanguage.JA:
                    return new CultureInfo("ja");
                case Requestify.ELanguage.KO:
                    return new CultureInfo("ko");
                case Requestify.ELanguage.NL:
                    return new CultureInfo("nl");
                case Requestify.ELanguage.NN:
                    return new CultureInfo("nn");
                case Requestify.ELanguage.PL:
                    return new CultureInfo("pl");
                case Requestify.ELanguage.BR:
                    return new CultureInfo("pt-BR");
                case Requestify.ELanguage.PT:
                    return new CultureInfo("pt");
                case Requestify.ELanguage.EN:
                    return new CultureInfo("en");
                case Requestify.ELanguage.RO:
                    return new CultureInfo("ro");
                case Requestify.ELanguage.RU:
                    return new CultureInfo("ru");
                case Requestify.ELanguage.SV:
                    return new CultureInfo("sv");
                case Requestify.ELanguage.TH:
                    return new CultureInfo("th");
                case Requestify.ELanguage.TR:
                    return new CultureInfo("tr");
                case Requestify.ELanguage.UK:
                    return new CultureInfo("uk");
                case Requestify.ELanguage.TZN:
                    return new CultureInfo("zh");
                case Requestify.ELanguage.SZN:
                    return new CultureInfo("zh-CN");
                default:
                    return new CultureInfo("en-US");
            }
        }
    }
}
