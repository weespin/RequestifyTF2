using System.Collections.Generic;
using RequestifyTF2.Api;

namespace RawPlugi
{
    public class RawPlugin : IRequestifyPlugin

    {
        public string Name => "Raw";
        public string Author => "Weespin";
        public string Help => "Playing a raw file. Translating mp3, streams, links, radio directly to VLC player.";
        public string Command => "!raw";
        public bool OnlyCode => false;

        public void Execute(string executor, List<string> arguments)
        {
            if (arguments.Count <= 0) return;
            var url = arguments[0];
            Instance.Vlc.Add(url);
        }
    }
}