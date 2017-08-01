using System;
using RequestifyTF2.Api;

namespace MTTSPlugin
{
    public class MttsPlugin : IPlugin

    {
        public string Name => "MTTS";

        public string Command => "!mtts";
        public bool OnlyCode => false;

        public double Version => 1;
        public string Author => "Weespin";

        public void Execute(string[] command, bool admin)
        {
            var i = 0;
            foreach (var a in command)
                if (a == "!mtts")
                    i++;
            if (i != 1)
                return;
            var index = Array.IndexOf(command, "!mtts");
            if (command.Length < index + 1)
                return;

            var text = "";

            for (var j = index + 1; j < command.Length; j++)
                text += command[j];

            var d =
                "http://cache-a.oddcast.com/c_fs/9587dd8632431aaff8bf03cfae0ff.mp3?engine=4&language=1&voice=5&text=" +
                text + "&useUTF8=1";
            d = d.Replace(" ", "%20");
            Instances.Vlc.Add(d);
        }
    }
}