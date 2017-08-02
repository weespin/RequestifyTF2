using System.Collections.Generic;
using RequestifyTF2.Api;

namespace TTSPlugin
{
    public class TtsPlugin : IRequestifyPlugin
    {
        public string Name => "TTS";

        public string Command => "!tts";

        public bool OnlyCode => false;

   


        public void Execute(string executor, List<string> arguments)
        {
            if (arguments.Count > 0)
            {
                var text = "";

                foreach (var texts in arguments)
                    text += texts;
                var d = "http://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=" +
                        text +
                        "&tl=En-gb";
                d = d.Replace(" ", "%20");

                Instances.Vlc.Add(d);
            }
        }
    }
}