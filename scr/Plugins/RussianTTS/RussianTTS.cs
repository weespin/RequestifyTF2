using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RequestifyTF2.Api;

namespace RTTSPlugin
{
    public class TtsPlugin : IRequestifyPlugin
    {
        public string Name => "RTTS";
        public string Author => "Weespin";
        public string Command => "!rtts";
        public string Help => "Playing a Russian Google voice";
        public bool OnlyCode => false;




      
        public void Execute(string executor, List<string> arguments)
        {
            if (arguments.Count > 0)
            {
                var text = arguments.Aggregate("", (current, texts) => current + texts);

                var d = "http://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=" +
                        text +
                        "&tl=Ru-ru";
                d = d.Replace(" ", "%20");

                Instance.Vlc.Add(d);
            }
        }
    }
}