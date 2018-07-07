using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LocalizationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Read();
            var a = File.ReadAllLines(@"C:\Program Files (x86)\Steam\steamapps\common\Team Fortress 2\tf\console.log");
            foreach (var line in a)
            {

                
                var utf8 = Encoding.GetEncoding("UTF-8");
                var win1251 = Encoding.GetEncoding("Windows-1251");

                var utf8Bytes = win1251.GetBytes(line);
                var win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
                var lin2 = win1251.GetString(win1251Bytes);
                Console.WriteLine(lin2);
            }

        }
        public static void Read()
        {
            var wh = new AutoResetEvent(false);
            var fsw = new FileSystemWatcher(".")
            {
                Filter = @"C:\Program Files (x86)\Steam\steamapps\common\Team Fortress 2\tf\console.log",
                EnableRaisingEvents = true
            };
            fsw.Changed += (s, e) => wh.Set();
           
            Thread.Sleep(30);


            var fs = new FileStream(
                @"C:\Program Files (x86)\Steam\steamapps\common\Team Fortress 2\tf\console.log",
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite);

            using (var sr = new StreamReader(fs, Encoding.GetEncoding("UTF-8")))
            {
                var s = string.Empty;
                while (true)
                {

                        s = sr.ReadLine();
                        if (!string.IsNullOrEmpty(s))
                    {
                        if (s.Contains("DllMain"))
                        {
                            var utf8 = Encoding.GetEncoding("UTF-8");
                            var win1251 = Encoding.GetEncoding("Windows-1251");

                            var utf8Bytes = win1251.GetBytes(s);
                            var win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
                            var ass = win1251.GetString(win1251Bytes);
                            Console.WriteLine(ass);
                        }

                    }
                        else
                        {
                            wh.WaitOne(25);
                        }
                    
                }
            }

        }
    }
}
