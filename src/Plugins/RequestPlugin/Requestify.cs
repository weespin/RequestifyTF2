using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CSCore;
using CSCore.Codecs.AAC;
using CSCore.Codecs.MP3;
using CSCore.SoundOut;
using Newtonsoft.Json;
using RequestifyTF2.API;
using RequestifyTF2.API.ConsoleAPI;
using RequestifyTF2.API.Events;
using RequestifyTF2.Audio;
using RequestifyTF2.DependencyLoader;
using YoutubeExplode;
using AudioEncoding = YoutubeExplode.Models.MediaStreams.AudioEncoding;

namespace RequestPlugin
{
    public class RequestPlugin : IRequestifyPlugin
    {
        public string Author => "Weespin";
        public string Name => "Requestify";
        public string Desc => "!request \"link\"";
    }
    public class VoteCommand : IRequestifyCommand
    {
        private readonly List<string> VoteUsers = new List<string>();
        private long MusicId;
        private int PlayersCount;
        public string Help => "Vote for skip!";
        public string Name => "voteskip";
        public bool OnlyAdmin => false;
        public List<string> Alias => new List<string>();

        public void Execute(User executor, List<string> arguments)
        {
            ConsoleSender.SendCommand("status", ConsoleSender.Command.Raw);
            Thread.Sleep(2000);
            if (AudioManager.BackGround.SoundOut.PlaybackState == PlaybackState.Playing)
            {
                if (MusicId != AudioManager.BackGround.SoundOut.WaveSource.Length)
                {
                    VoteUsers.Clear();
                    MusicId = AudioManager.BackGround.SoundOut.WaveSource.Length;
                    VoteUsers.Add(executor.Name);
                    if (!Instance.IsMuted)
                    {
                        ConsoleSender.SendCommand(
                            $"{executor} voted to skip this song. {VoteUsers.Count}/{PlayersCount}",
                            ConsoleSender.Command.Chat);
                    }

                    if (VoteUsers.Count >= PlayersCount / 2)
                    {
                        AudioManager.BackGround.SoundOut.Stop();
                        if (!Instance.IsMuted)
                        {
                            ConsoleSender.SendCommand($"This song has been skipped", ConsoleSender.Command.Chat);
                        }
                    }
                }
                else
                {
                    if (VoteUsers.Count >= PlayersCount / 2)
                    {
                        AudioManager.BackGround.SoundOut.Stop();
                        if (!Instance.IsMuted)
                        {
                            ConsoleSender.SendCommand($"This song has been skipped", ConsoleSender.Command.Chat);
                        }
                    }

                    if (VoteUsers.Contains(executor.Name))
                    {
                        if (!Instance.IsMuted)
                        {
                            ConsoleSender.SendCommand(
                                $"{executor} already voted to skip this song. {VoteUsers.Count}/{PlayersCount}",
                                ConsoleSender.Command.Chat);
                        }
                    }
                    else
                    {
                        ConsoleSender.SendCommand(
                            $"{executor} voted to skip this song. {VoteUsers.Count}/{PlayersCount}",
                            ConsoleSender.Command.Chat);
                        if (VoteUsers.Count >= PlayersCount)
                        {
                            AudioManager.BackGround.SoundOut.Stop();
                            if (!Instance.IsMuted)
                            {
                                ConsoleSender.SendCommand($"This song has been skipped", ConsoleSender.Command.Chat);
                            }
                        }
                    }
                }
            }
            else
            {
                if (!Instance.IsMuted)
                {
                    ConsoleSender.SendCommand($"{executor}, the queue is empty.", ConsoleSender.Command.Chat);
                }
            }
        }

        public void OnLoad()
        {
            using (var web = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
                                                       SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

              
                if (!File.Exists("./lib/YoutubeExplode.dll"))
                {
                    web.DownloadFile(
                        "https://github.com/weespin/reqdeps/blob/master/YoutubeExplode.dll?raw=true",
                        "./lib/YoutubeExplode.dll");
                    DependencyLoader.LoadFile("./lib/YoutubeExplode.dll");
                }

                if (!File.Exists("./lib/AngleSharp.dll"))
                {
                    web.DownloadFile(
                        "https://github.com/weespin/reqdeps/blob/master/AngleSharp.dll?raw=true",
                        "./lib/AngleSharp.dll");
                    DependencyLoader.LoadFile("./lib/AngleSharp.dll");
                }
            }

            Events.OnUndefinedMessage += OnUndef;
        }

        private void OnUndef(RequestifyEventArgs.UndefinedMessageArgs e)
        {
            var reg = new Regex(@"players : (\d+) humans, (\d+) bots \((\d+) max\)").Match(e.Message);
            if (reg.Success)
            {
                PlayersCount = int.Parse(reg.Groups[1].Value);
            }
        }

        public class StopCommand : IRequestifyCommand
        {
            public string Author => "Weespin";

            public string Command => "!stop";

            public string Help => "Delete all background queue and stop current music!";

            public string Name => "stop";

            public List<string> Alias => new List<string>();


            public bool OnlyAdmin => true;

            public void Execute(User executor, List<string> arguments)
            {
                if (executor.Name != Instance.Admin)
                {
                    return;
                }

                if (arguments[0] == "cur")
                {
                    AudioManager.BackGround.SoundOut.Stop();
                    return;
                }

                if (arguments[0] == "que")
                {
                    AudioManager.BackGround.PlayList = new ConcurrentQueue<AudioManager.Song>();
                    return;
                }

                AudioManager.BackGround.SoundOut.Stop();
                AudioManager.BackGround.PlayList = new ConcurrentQueue<AudioManager.Song>();
            }
        }

        public class RequestCommand : IRequestifyCommand
        {
            private static readonly int AppID = 1505226596;

            private static readonly string clientID = "WKcQQdEZw7Oi01KqtHWxeVSxNyRzgT8M";


            public string Help => "Play music. Supports soundcloud and youtube!";

            public string Name => "request";
            public List<string> Alias => new List<string>();

            public bool OnlyAdmin => false;

            public void Execute(User executor, List<string> arguments)
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
                                var durl = web.DownloadString(
                                    "https://api.soundcloud.com/tracks/" + b.id + "/streams?client_id=" + clientID
                                    + "&app_version=" + AppID);
                                var urls = JsonConvert.DeserializeObject<DownloadURL>(durl);
                                if (urls.http_mp3_128_url != null)
                                {
                                    if (!Instance.IsMuted)
                                    {
                                        ConsoleSender.SendCommand(
                                            $"{b.title} was added to the queue",
                                            ConsoleSender.Command.Chat);
                                    }

                                    AudioManager.BackGround.PlayList.Enqueue(
                                        new AudioManager.Song(
                                            b.title,
                                            new Mp3MediafoundationDecoder(urls.http_mp3_128_url).ToMono(),
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
                    var streamInfo =
                        streamInfoSet.Result.Audio.FirstOrDefault(n => n.AudioEncoding == AudioEncoding.Aac);
                    if (streamInfo == null)
                    {
                        return;
                    }

                    var ext = streamInfo.Url;
                    var title = client.GetVideoAsync(id).Result.Title;
                    if (!Instance.IsMuted)
                    {
                        ConsoleSender.SendCommand($"{title} was added to the queue", ConsoleSender.Command.Chat);
                    }

              AudioManager.BackGround.PlayList.Enqueue(new AudioManager.Song(title, new AacDecoder(ext), executor));
                }
                else
                {
                    if (arguments.Count >= 1)
                    {
                        var text = arguments.Aggregate(" ", (current, argument) => current + " " + argument);
                        var client = new YoutubeClient();
                        var vids = client.SearchVideosAsync(text,1).Result;
                        if (vids.Count > 0)
                        {
                            var streamInfoSet = client.GetVideoMediaStreamInfosAsync(vids[0].Id);
                            var streamInfo =
                                streamInfoSet.Result.Audio.FirstOrDefault(n => n.AudioEncoding == AudioEncoding.Aac);
                            if (streamInfo == null)
                            {
                                return;
                            }

                            var ext = streamInfo.Url;
                            var title = client.GetVideoAsync(vids[0].Id).Result.Title;
                            if (!Instance.IsMuted)
                            {
                                ConsoleSender.SendCommand($"{title} was added to the queue",
                                    ConsoleSender.Command.Chat);

                            }

                            AudioManager.BackGround.PlayList.Enqueue(new AudioManager.Song(title, new AacDecoder(ext).ToMono(),
                                executor));
                        }
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
}