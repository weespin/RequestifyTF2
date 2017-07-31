using System.IO;
using System.Net;

namespace RequestifyTF2.Utils
{
    public class AutoHotKeyChecker
    {
        public void Check(string path)

        {
            if (Directory.Exists(path + "/ahk/"))
            {
                if (!File.Exists(path + "/ahk/ahk.exe"))
                    using (var web = new WebClient())
                    {
                        web.Proxy = null;
                        web.DownloadFile("http://earrapify.com/requestifyapi/ahk/ahk.exe", path + "/ahk/ahk.exe");
                    }
            }
            else
            {
                Directory.CreateDirectory(path + "/ahk/");
                using (var web = new WebClient())
                {
                    web.Proxy = null;
                    web.DownloadFile("http://earrapify.com/requestifyapi/ahk/ahk.exe", path + "/ahk/ahk.exe");
                }
            }
        }
    }
}