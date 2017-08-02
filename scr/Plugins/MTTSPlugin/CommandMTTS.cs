using System;
using System.Collections.Generic;
using RequestifyTF2.Api;

namespace MTTSPlugin
{
    public class MttsPlugin : IRequestifyPlugin

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
                var d =
                    "http://cache-a.oddcast.com/c_fs/9587dd8632431aaff8bf03cfae0ff.mp3?engine=4&language=1&voice=5&text=" +
                    text + "&useUTF8=1";
                d = d.Replace(" ", "%20");

                Instances.Vlc.Add(d);
            }
        }

        public void OnLoad()
        {
            
        }
    }
}