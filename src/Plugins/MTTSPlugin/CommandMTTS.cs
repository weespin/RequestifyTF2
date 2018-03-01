namespace MTTSPlugin
{
    using System.Collections.Generic;
    using System.Linq;

    using CSCore.Codecs.MP3;

    using RequestifyTF2.Api;

    public class MTTSPlugin : IRequestifyPlugin
    {
        public string Author => "Weespin";
        public string Name => "WillfromAfar TTS";
        public string Desc => "mtts \"text\"";
    }
    public class MttsPlugin : IRequestifyCommand
    {

        public string Help => "Playing a David UK (MLG) voice";

        public string Name => "mtts";

        public bool OnlyAdmin => false;
        public List<string> Alias => new List<string>();
        public string Desc { get; }

        public bool OnlyCode => false;

        public void Execute(string executor, List<string> arguments)
        {
            if (arguments.Count > 0)
            {
                var text = arguments.Aggregate(" ", (current, argument) => current + " " + argument);

                var d =
                    "http://cache-a.oddcast.com/c_fs/9587dd8632431aaff8bf03cfae0ff.mp3?engine=4&language=1&voice=5&text="
                    + text + "&useUTF8=1";
                d = d.Replace(" ", "%20");
                Instance.QueueForeGround.Enqueue(new Mp3MediafoundationDecoder(d));
            }
        }
    }
}