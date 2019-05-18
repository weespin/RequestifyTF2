using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using CSCore.Codecs.MP3;
using RequestifyTF2.API;

namespace TTSPlugin
{
    public class TTSPlugin : IRequestifyPlugin
    {
        public string Author => "Weespin";
        public string Name => "TTS";
        public string Desc => "tts \"text\"";
    }

    public class MttsPlugin : IRequestifyCommand
    {
        private const int engine = 4;
        private const int lang = 1;
        private const int level1 = 0;
        private const int level2 = 0;
        private const int voice = 5;
        private const int accid = 5883747;
        private const string secret = "uetivb9tb8108wfj";
        public string Help => "Playing a David UK (MLG) voice";

        public string Name => "mtts";
        public bool OnlyAdmin => false;
        public List<string> Alias => new List<string>();

        public void Execute(User executor, List<string> arguments)
        {
            if (arguments.Count > 0)
            {
                var text = arguments.Aggregate(" ", (current, argument) => current + " " + argument);
                var magic = $"{engine}{lang}{voice}{text}1mp3{accid}{secret}";
                var checksum = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(magic));
                var stringchecksum = string.Concat(Array.ConvertAll(checksum, x => x.ToString("X2")));
                var url =
                    $"http://cache-a.oddcast.com/tts/gen.php?EID={engine}&LID={lang}&VID={voice}&TXT={text}&IS_UTF8=1&EXT=mp3&FNAME=&ACC={accid}&API=&SESSION=&CS={stringchecksum}&cache_flag=3";

                Instance.QueueForeGround.Enqueue(new Mp3MediafoundationDecoder(url));
            }
        }
    }

    public class GTTSCommand : IRequestifyCommand
    {
        public string Help => "Playing a WillFromAfar (purple sheep) voice";


        public string Name => "gtts";
        public bool OnlyAdmin => false;
        public List<string> Alias => new List<string>();

        public void Execute(User executor, List<string> arguments)
        {
            if (arguments.Count > 0)
            {
                var text = arguments.Aggregate(" ", (current, argument) => current + " " + argument);
                var link = Parse(text, "willfromafar22k_hq");
                if (link == "")
                {
                    return;
                }

                Instance.QueueForeGround.Enqueue(new Mp3MediafoundationDecoder(link));
            }
        }

        public static string Parse(string text, string voiceid)
        {
            voiceid = voiceid.Replace("22k_hq", "_22k_ns.bvcu");
            var client = new HttpClient();
            var rnd = new Random();
            var length = 20;
            var str = "{\"googleid\":\"";
            var email = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                email.Append(((char) (rnd.Next(1, 26) + 64)).ToString());
            }

            email.Append("@gmail.com");
            var values = new Dictionary<string, string>
            {
                {"json", str + email + "\"}"}
            };

            var content = new FormUrlEncodedContent(values);
            var response = client.PostAsync("https://acapelavoices.acapela-group.com/index/getnonce/", content).Result
                .Content.ReadAsStringAsync().Result;
            var re = new Regex(@"^\{\""nonce\""\:\""(.+)\""\}$");
            var m = re.Match(response);
            if (m.Groups.Count > 1)
            {
                var request =
                    (HttpWebRequest) WebRequest.Create(
                        "http://www.acapela-group.com:8080/webservices/1-34-01-Mobility/Synthesizer");
                var enc =
                    $"req_voice=enu_{voiceid}&cl_pwd=&cl_vers=1-30&req_echo=ON&cl_login=AcapelaGroup&req_comment=%7B%22nonce%22%3A%22{m.Groups[1]}%22%2C%22user%22%3A%22{email}%22%7D&req_text={Uri.EscapeDataString(text)}&cl_env=ACAPELA_VOICES&prot_vers=2&cl_app=AcapelaGroup_WebDemo_Android";

                var data = Encoding.ASCII.GetBytes(enc);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var responses = (HttpWebResponse) request.GetResponse();

                var responseString = new StreamReader(responses.GetResponseStream()).ReadToEnd();
                var reg = new Regex("snd_url=(.+)&snd_size");
                var regs = reg.Match(responseString);
                if (regs.Success)
                {
                    return regs.Groups[1].Value;
                }

                return "";
            }

            return "";
        }
    }

    public class TTSCommand : IRequestifyCommand
    {
        public string Help => "Playing a Google voice";

        public string Name => "tts";
        public bool OnlyAdmin => false;
        public List<string> Alias => new List<string>();

        public void Execute(User executor, List<string> arguments)
        {
            if (arguments.Count > 0)
            {
                var text = arguments.Aggregate(" ", (current, argument) => current + " " + argument);

                if (Regex.IsMatch(text, @"\p{IsCyrillic}"))
                {
                    text = HttpUtility.UrlEncode(text);
                    var f =
                        "http://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q="
                        + text + "&tl=Ru-ru";
                    f = f.Replace(" ", "%20");

                    Instance.QueueForeGround.Enqueue(new Mp3MediafoundationDecoder(f));
                    return;
                }

                text = HttpUtility.UrlEncode(text);
                var d = "http://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q="
                        + text + "&tl=En-gb";
                d = d.Replace(" ", "%20");
                Instance.QueueForeGround.Enqueue(new Mp3MediafoundationDecoder(d));
            }
        }
    }
}