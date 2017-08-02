using System;
using System.Collections.Generic;
using System.Linq;
using RequestifyTF2.Api;

namespace RequestPlugin
{
    public class RequestPlugin :IRequestifyPlugin

    {
        public string Name => "Request";

        public string Command => "!request";
        public bool OnlyCode => false;

        public double Version => 1.2;
        public string Author => "Weespin";

        public void Execute(string executor, List<string> arguments)
        {
            if (arguments.Count > 0)
            {
                var url = arguments[0];
                if (url.StartsWith("https://soundcloud.com/"))
                    Instances.Vlc.Add(url);
                if (url.StartsWith("https://www.youtube.com/watch?v="))
                    Instances.Vlc.Add(url);
                if (url.StartsWith("https://youtu.be/"))
                   Instances.Vlc.Add(url);       
            }
        }

     
    }
}