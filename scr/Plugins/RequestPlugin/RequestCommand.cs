using System.Collections.Generic;
using RequestifyTF2.Api;

namespace RequestPlugin
{
    public class RequestPlugin : IRequestifyPlugin

    {
        public string Name => "Request";

        public string Command => "!request";
        public bool OnlyCode => false;



        public void Execute(string executor, List<string> arguments)
        {
            if (arguments.Count > 0)
            {
                var url = arguments[0];
                if (url.StartsWith("https://soundcloud.com/"))
                    Instances.Vlc.Add(url);
                if (url.StartsWith("https://www.youtube.com/watch?v="))
                    Instances.Vlc.Add(url);
                if (url.Contains("bandcamp"))
                {
                    // const string rgxIsTrackPath = "^http://[a-z0-9\\-]+?\\.bandcamp\\.com/track/[a-z0-9\\-]+?/?$";
                }
                if (url.StartsWith("https://youtu.be/"))
                    Instances.Vlc.Add(url);
            }
        }
    }
}