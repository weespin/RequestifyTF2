using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using CSCore;
using Newtonsoft.Json;
using RequestifyTF2.Api;

namespace GTTS
{
    public class Plugin : IRequestifyPlugin
    {
        public string Name => "GTTS";
        public string Author => "Weespin";
        public string Help => "Playing a WillFromAfar (purple sheep) voice";
        public string Command => "!gtts";
        public bool OnlyCode => false;



        public void Execute(string executor, List<string> arguments)
        {
            if (arguments.Count > 0)
            {
                string query = "https://acapela-box.com/AcaBox/index0.php?cookietest=2";

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(query);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                var resps = resp.Headers["Set-Cookie"];
                var regex = @"(acabox=)\w+";
                var result = "";
                var match = Regex.Match(resps, regex);
                if (match.Success)
                {
                    result = match.Value;

                }
                else
                {
                    return;
                }
                var text = arguments.Aggregate(" ", (current, argument) => current + " " + argument);

                text = text.Replace(" ", "%20");
                var request = (HttpWebRequest) WebRequest.Create("https://acapela-box.com/AcaBox/dovaas.php");

                text = text.Replace(" ", "%20");
                var postData =
                    $"text=%5Cvct%3D100%5C%20%5Cspd%3D180%5C%20{text}&voice=willfromafar22k&listen=1&format=MP3&codecMP3=1&spd=180&vct=100";

                var data = Encoding.ASCII.GetBytes(postData);
                request.Accept = "application/json, text/javascript, */*; q=0.01";

                request.Headers.Add("Origin", "https://acapela-box.com");
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                request.UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.78 Safari/537.36";
                request.Referer = "https://acapela-box.com/AcaBox/index.php";
                request.Headers.Add("Cookie", result);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                { 
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse) request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var s = JsonConvert.DeserializeObject<AcapellaResp>(responseString).snd_url;
                
                Instance.QueueForeGround.Enqueue(new CSCore.Codecs.MP3.Mp3MediafoundationDecoder(s));
            }
        }
    }

    public class AcapellaResp
    {
        public string snd_url { get; set; }
    }
}