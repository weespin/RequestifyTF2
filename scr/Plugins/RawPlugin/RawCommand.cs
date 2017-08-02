using System.Collections.Generic;
using RequestifyTF2.Api;

namespace RawPlugi
{
    public class RawPlugin : IRequestifyPlugin

    {
        public string Name => "Raw";

        public string Command => "!raw";
        public bool OnlyCode => false;

        public double Version => 1.2;
        public string Author => "Weespin";

        public void Execute(string executor, List<string> arguments)
        {
            if (arguments.Count > 0)
            {
                var url = arguments[0];
                Instances.Vlc.Add(url);
            }
        }
    }
}