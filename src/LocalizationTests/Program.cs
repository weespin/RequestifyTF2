using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LocalizationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            if (new Regex($"{Localization.Localization.TF_CHAT_DEAD.Replace("*", "\\*")}").Match("sdadas *DEAD* (TEAM) Hi: girls").Success)
            {
                Console.WriteLine("OK");
            }
        
       
            Console.WriteLine(Localization.Localization.TF_CHAT_TEAM);
            Console.Read();
        }
    }
}
