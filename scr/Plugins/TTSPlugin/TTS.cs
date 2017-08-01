using System;
using System.Linq;
using RequestifyTF2.Api;

namespace TTSPlugin
{
    public class TtsPlugin : IPlugin
    {
        public string Name => "TTS";

        public string Command => "!tts";

        public bool OnlyCode => false;

        public double Version => 1;
        public string Author => "Weespin";

        public void Execute(string[] command, bool admin)
        {
            var i = command.Count(a => a == Command);
            if (i != 1)
                return;
            var index = Array.IndexOf(command, Command);
            if (command.Length < index + 1)
                return;

            var text = "";

            for (var j = index + 1; j < command.Length; j++)
                text += command[j];

            var d = "http://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=" +
                    text +
                    "&tl=En-gb";
            d = d.Replace(" ", "%20");

            Instances.Vlc.Add(d);
        }
    }
}