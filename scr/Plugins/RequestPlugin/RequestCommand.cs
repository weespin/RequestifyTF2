using System.Collections.Generic;
using RequestifyTF2.Api;

namespace RequestPlugin
{
    public class RequestPlugin : IRequestifyPlugin

    {
        public string Name => "Request";
        public string Author => "Weespin";
        public string Command => "!request";
        public string Help => "Play music. Supports soundcloud and youtube!";
        public bool OnlyCode => false;



        public void Execute(string executor, List<string> arguments)
        {
            if (arguments.Count <= 0)
            {
                return;

            }
            var url = arguments[0];
            if (url.StartsWith("https://soundcloud.com/"))
                Instance.Vlc.Add(url);
            if (url.StartsWith("https://www.youtube.com/watch?v="))
                Instance.Vlc.Add(url);
            if (url.StartsWith("https://youtu.be/"))
                Instance.Vlc.Add(url);
        }
    }
}