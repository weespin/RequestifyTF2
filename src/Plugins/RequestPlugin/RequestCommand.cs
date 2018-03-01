namespace RequestPlugin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;

    using CSCore.Codecs.AAC;
    using CSCore.Codecs.MP3;

    using Newtonsoft.Json;

    using RequestifyTF2.Api;
    using RequestifyTF2.PluginLoader;

    using YoutubeExplode;
    using YoutubeExplode.Models.MediaStreams;
    public class RequestPlugin : IRequestifyPlugin
    {
        public string Author => "Weespin";
        public string Name => "Requestify";
        public string Desc => "!request \"link\"";
    }
    public class RequestCommand : IRequestifyCommand
    {
        private static readonly int AppID = 1505226596;

        private static readonly string clientID = "WKcQQdEZw7Oi01KqtHWxeVSxNyRzgT8M";

      

        public string Help => "Play music. Supports soundcloud and youtube!";

        public string Name => "Request";
        public List<string> Alias { get; }

        public bool OnlyAdmin => false;

        public void Execute(string executor, List<string> arguments)
        {
            if (arguments.Count <= 0)
            {
                return;
            }

            var url = arguments[0];

            var regex = new Regex(@"^(https?:\/\/)?(www.)?soundcloud\.com\/[\w\-\.]+(\/)+[\w\-\.]+/?$");
            if (regex.Match(url).Success)
            {
                try
                {
                    using (var web = new WebClient())
                    {
                        var info = web.DownloadString(
                            "http://api.soundcloud.com/resolve.json?url=" + url + "&client_id=" + clientID
                            + "&app_version=" + AppID);
                        var b = JsonConvert.DeserializeObject<Track>(info);
                        if (b.streamable && b.kind == "track")
                        {
                            string durl = web.DownloadString(
                                "https://api.soundcloud.com/tracks/" + b.id + "/streams?client_id=" + clientID
                                + "&app_version=" + AppID);
                            var urls = JsonConvert.DeserializeObject<DownloadURL>(durl);
                            if (urls.http_mp3_128_url != null)
                            {
                                ConsoleSender.SendCommand(
                                    $"{b.title} was added to the queue",
                                    ConsoleSender.Command.Chat);
                                Instance.BackGroundQueue.PlayList.Enqueue(
                                    new Instance.Song(
                                        b.title,
                                        new Mp3MediafoundationDecoder(urls.http_mp3_128_url),
                                        executor));

                                return;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }

            

            var youtube = new Regex(@"youtube\..+?/watch.*?v=(.*?)(?:&|/|$)");
            var shortregex = new Regex(@"youtu\.be/(.*?)(?:\?|&|/|$)");
            if (youtube.Match(url).Success || shortregex.Match(url).Success)
            {
                var id = YoutubeClient.ParseVideoId(url);
                var client = new YoutubeClient();
                var streamInfoSet = client.GetVideoMediaStreamInfosAsync(id);
                var streamInfo = streamInfoSet.Result.Audio.FirstOrDefault(n => n.AudioEncoding == AudioEncoding.Aac);
                if (streamInfo == null)
                {
                    return;
                }

                var ext = streamInfo.Url;
                var title = client.GetVideoAsync(id).Result.Title;
                ConsoleSender.SendCommand($"{title} was added to the queue", ConsoleSender.Command.Chat);
                Instance.BackGroundQueue.PlayList.Enqueue(new Instance.Song(title, new AacDecoder(ext), executor));
            }

            
        }

        public void OnLoad()
        {
            using (WebClient web = new WebClient())
            {
                // I want to check libs
                if (!File.Exists("./plugins/libs/YoutubeExplode.dll"))
                {
                    web.DownloadFile(
                        "https://github.com/weespin/reqdeps/blob/master/YoutubeExplode.dll?raw=true",
                        "./plugins/libs/YoutubeExplode.dll");
                    Libraries.LoadFile("./plugins/libs/YoutubeExplode.dll");
                }

                if (!File.Exists("./plugins/libs/AngleSharp.dll"))
                {
                    web.DownloadFile(
                        "https://github.com/weespin/reqdeps/blob/master/AngleSharp.dll?raw=true",
                        "./plugins/libs/AngleSharp.dll");
                    Libraries.LoadFile("./plugins/libs/AngleSharp.dll");
                }
            }
        }

        public class DownloadURL
        {
            public string http_mp3_128_url { get; set; }
        }

        public class Track
        {
            public int id { get; set; }

            public string kind { get; set; }

            public bool streamable { get; set; }

            public string title { get; set; }
        }
    }
}