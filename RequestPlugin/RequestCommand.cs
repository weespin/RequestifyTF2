using System;
using System.Linq;
using RequestifyTF2.Api;

namespace RequestPlugin
{
    public class RequestPlugin : IPlugin

    {
        public string Name => "Request";

        public string Command => "!request";
        public bool OnlyCode => false;

        public double Version => 1.1;
        public string Author => "Cancer";

        public void Execute(string[] command, bool admin)
        {
            var url = string.Empty;
            var i = command.Count(a => a == "!request");
            if (i != 1)
                return;
            var index = Array.IndexOf(command, "!request");
            try
            {
                url = command[index + 1];
            }
            catch (Exception)
            {
                return;
            }
            if (url == "")
            {
                return;
                ;
            }
            if (command.Length < index + 1)
                return;
            if (url.StartsWith("https://soundcloud.com/"))
                Instances.Vlc.Add(command[index + 1]);
            if (url.StartsWith("https://www.youtube.com/watch?v="))
                Instances.Vlc.Add(command[index + 1]);
        }
    }
}