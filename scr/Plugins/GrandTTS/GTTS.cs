using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RequestifyTF2.Api;

namespace GTTS
{
    public class Plugin : IRequestifyPlugin
    {
        public string Name => "MTTS";

        public string Command => "!mtts";
        public bool OnlyCode => false;

        public double Version => 1;
        public string Author => "Weespin";
        public void Execute(string executor, List<string> arguments)
        {
            if (arguments.Count > 0)
            {
                var text = "";

                foreach (var texts in arguments)
                {
                    text += texts;
                }
             
                text =text.Replace(" ", "%20");
                var request = (HttpWebRequest)WebRequest.Create("https://acapela-box.com/AcaBox/dovaas.php");
               
                text = text.Replace(" ", "%20");
                var postData = $"text=%5Cvct%3D100%5C%20%5Cspd%3D180%5C%20{text}&voice=willfromafar22k&listen=1&format=MP3&codecMP3=1&spd=180&vct=100";

                var data = Encoding.ASCII.GetBytes(postData);
                request.Accept = "application/json, text/javascript, */*; q=0.01";

                request.Headers.Add("Origin", "https://acapela-box.com");
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.78 Safari/537.36";
                request.Referer = "https://acapela-box.com/AcaBox/index.php";
                request.Headers.Add("Cookie", "acabox=90hmddj8mahttpigipr33lsf73");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var s = JsonConvert.DeserializeObject<RootObject>(responseString).snd_url;
                Instances.Vlc.Add(s);
            }
        }
    }
    public class RootObject
    {
        public string snd_time { get; set; }
        public string snd_id { get; set; }
        public string snd_url { get; set; }
        public string snd_size { get; set; }
        public string res { get; set; }
        public string create_echo { get; set; }
        public string req_comment { get; set; }
        public string req_text { get; set; }
        public string req_voice { get; set; }
        public string req_snd_kbps { get; set; }
        public string cl_ip { get; set; }
        public string req_snd_type { get; set; }
        public string req_vol { get; set; }
        public string req_spd { get; set; }
        public string req_vct { get; set; }
        public int cost { get; set; }
        public string replaced { get; set; }
    }
}
