using System.Collections.Generic;
using System.Linq;
using CSCore.Codecs.AAC;
using CSCore.Codecs.MP3;
using RequestifyTF2.Api;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

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
            //if (url.StartsWith("https://soundcloud.com/"))
            //    Instance.Vlc.Add(url);
            if (url.StartsWith("https://www.youtube.com/watch?v=") || url.StartsWith("https://youtu.be/"))
            {
                var id = YoutubeClient.ParseVideoId(url); // "bnsUkE8i0tU"
                var client = new YoutubeClient();
                var streamInfoSet = client.GetVideoMediaStreamInfosAsync(id);
                var streamInfo = streamInfoSet.Result.Audio
                    .FirstOrDefault(n => n.AudioEncoding == AudioEncoding.Aac);
                if (streamInfo == null)
                {
                    return;
                }

                var ext = streamInfo.Url;
                Instance.QueueForeGround.Enqueue(new AacDecoder(ext));
            }
               
            //if (url.StartsWith("https://youtu.be/"))
            //    Instance.Vlc.Add(url);
        }
    }
}