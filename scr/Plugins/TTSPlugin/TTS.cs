using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using CSCore.Codecs.MP3;
using RequestifyTF2.Api;

namespace TTSPlugin
{
    public class TtsPlugin : IRequestifyPlugin
    {
        public string Name => "TTS";
        public string Author => "Weespin";
        public string Command => "!tts";
        public string Help => "Playing a Google voice";
        public bool OnlyCode => false;

   


        public void Execute(string executor, List<string> arguments)
        {
            if (arguments.Count > 0)
            {
                var text = arguments.Aggregate(" ", (current, argument) => current + " " + argument);
               
                if (Regex.IsMatch(text, @"\p{IsCyrillic}"))
                {
                    text = HttpUtility.UrlEncode(text);
                    var f = "http://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=" +
                            text +
                            "&tl=Ru-ru";
                    f = f.Replace(" ", "%20");
                 
                    Instance.QueueForeGround.Enqueue(new Mp3MediafoundationDecoder(f));
                    return;
                }
                text = HttpUtility.UrlEncode(text);
                var d = "http://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=" +
                        text +
                        "&tl=En-gb";
                d = d.Replace(" ", "%20");
                Instance.QueueForeGround.Enqueue(new Mp3MediafoundationDecoder(d));

            }
        }
    }
}