using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using CSCore.Codecs.AAC;
using CSCore.Codecs.MP3;
using CSCore.SoundOut;
using Newtonsoft.Json;
using RequestifyTF2.API;
using RequestifyTF2.PluginLoader;
using YoutubeExplode;
using AudioEncoding = YoutubeExplode.Models.MediaStreams.AudioEncoding;

namespace RequestPlugin
{
    public class RequestPlugin : IRequestifyPlugin
    {
       
        public static RequestPlugin instance;
        public string Author => "Weespin";
        public string Name => "Requestify";
        public string Desc => "!request \"link\"";
        

    }

    public class VoteCommand : IRequestifyCommand
    {

      
        public string Help => "Vote for skip!";
        public string Name => "voteskip";
        public  static int PlayersCount = 0;
        public readonly List<string> VoteUsers = new List<string>();
        public  static long MusicId;

        public bool OnlyAdmin => false;
        public List<string> Alias => new List<string>();

        public void Execute(User executor, List<string> arguments)
        {
            ConsoleSender.SendCommand("status", ConsoleSender.Command.Raw);
            Thread.Sleep(2000);
            if (Instance.SoundOutBackground.PlaybackState == PlaybackState.Playing)
            {
                if (MusicId != Instance.SoundOutBackground.WaveSource.Length)
                {
                    VoteUsers.Clear();
                    MusicId = Instance.SoundOutBackground.WaveSource.Length;
                    VoteUsers.Add(executor.Name);

                    var pl = PlayersCount < 4 ?  1 : PlayersCount / 2 ;
                    ConsoleSender.SendCommand(
                            $"{executor.Name} voted to skip this song. { VoteUsers.Count}/{pl}",
                            ConsoleSender.Command.Chat);
                    

                    if (VoteUsers.Count >= PlayersCount / 4)
                    {
                        Instance.SoundOutBackground.Stop();
                       
                            ConsoleSender.SendCommand($"This song has been skipped", ConsoleSender.Command.Chat);
                        
                    }
                }
                else
                {
                    if (VoteUsers.Count >= PlayersCount / 4)
                    {
                        Instance.SoundOutBackground.Stop();
                       
                            ConsoleSender.SendCommand($"This song has been skipped", ConsoleSender.Command.Chat);
                        
                    }

                    if (VoteUsers.Contains(executor.Name))
                    {
                        
                            ConsoleSender.SendCommand(
                                $"{executor.Name} already voted to skip this song. { VoteUsers.Count}/{PlayersCount}",
                                ConsoleSender.Command.Chat);
                        
                    }
                    else
                    {
                        ConsoleSender.SendCommand(
                            $"{executor.Name} voted to skip this song. {VoteUsers.Count}/{PlayersCount}",
                            ConsoleSender.Command.Chat);
                        if (VoteUsers.Count >= PlayersCount)
                        {
                            Instance.SoundOutBackground.Stop();
                            
                                ConsoleSender.SendCommand($"This song has been skipped", ConsoleSender.Command.Chat);
                            
                        }
                    }
                }
            }
            else
            {
               
                    ConsoleSender.SendCommand($"{executor.Name}, the queue is empty.", ConsoleSender.Command.Chat);
                
            }
        }

        public void OnLoad()
        {
            using (var web = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
                                                       SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                // I want to check libs
                if (!File.Exists("./lib/YoutubeExplode.dll"))
                {
                    web.DownloadFile(
                        "https://github.com/weespin/reqdeps/blob/master/YoutubeExplode.dll?raw=true",
                        "./lib/YoutubeExplode.dll");
                    Libraries.LoadFile("./lib/YoutubeExplode.dll");
                }

                if (!File.Exists("./lib/AngleSharp.dll"))
                {
                    web.DownloadFile(
                        "https://github.com/weespin/reqdeps/blob/master/AngleSharp.dll?raw=true",
                        "./lib/AngleSharp.dll");
                    Libraries.LoadFile("./lib/AngleSharp.dll");
                }
            }

            Events.UndefinedMessage.OnUndefinedMessage += OnUndef;
        }

        private void OnUndef(Events.UndefinedMessageArgs e)
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
                if (executor.Name != Instance.Config.Admin)
                {
                    return;
                }
                Thread.Sleep(800);
                if (arguments.Count > 0)
                {
                    if (arguments[0] == "cur")
                    {
                        Instance.SoundOutBackground.Stop();
                        ConsoleSender.SendCommand($"Skipped a song, {Instance.BackGroundQueue.PlayList.Count} in queue",ConsoleSender.Command.Chat);
                        return;
                    }

                    if (arguments[0] == "que")
                    {
                        Instance.BackGroundQueue.PlayList = new ConcurrentQueue<Instance.Song>();
                        ConsoleSender.SendCommand("Done, now playlist is clear", ConsoleSender.Command.Chat);
                        return;
                    }

                }

                Instance.SoundOutBackground.Stop();
                Instance.BackGroundQueue.PlayList = new ConcurrentQueue<Instance.Song>();
                ConsoleSender.SendCommand("BackGroundQueue is now clear", ConsoleSender.Command.Chat);
            }
        }

        public class RequestCommand : IRequestifyCommand
        {
            private static readonly int AppID = 1553518929;

            private static readonly string clientID = "NxDq1GKZ5tLDRohQGfJ7lYVKiephsF3G";


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

                var soundcloudregex = new Regex(@"^(https?:\/\/)?(www.)?soundcloud\.com\/[\w\-\.]+(\/)+[\w\-\.]+/?$");
                if (soundcloudregex.Match(url).Success)
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

                                    Instance.BackgroundEnqueue(Instance.SongType.MP3, urls.http_mp3_128_url,
                                        executor.Name, b.title);
                                   

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
                    ConsoleSender.SendCommand($"{title} was added to the queue", ConsoleSender.Command.Chat);

                    Instance.BackgroundEnqueue(Instance.SongType.AAC, ext,
                        executor.Name, title);
                    
                }
                else
                {
                    var text = arguments.Aggregate(" ", (current, argument) => current + " " + argument);
                    var client = new YoutubeClient();
                    var vids = client.SearchVideosAsync(text, 1).Result;
                    if (vids.Count > 0)
                    {
                        for (int i = 0; i < vids.Count; i++)
                        {
                            var streamInfoSet = client.GetVideoMediaStreamInfosAsync(vids[i].Id).ConfigureAwait(false).GetAwaiter().GetResult();
                            var ext = "";
                           
                            foreach (var ad in streamInfoSet.Audio)
                            {
                                if (Enum.GetName(typeof(AudioEncoding),ad.AudioEncoding)=="Aac") //Msbuild fuck upped this moment. I cant compare different enums. MSBUILD 16 VS19 RC3
                                {
                                    ext = ad.Url;
                                    break;
                                }
                            }
                            if (ext == string.Empty)
                            {
                                continue;
                            }
                            
                         
                            var title = client.GetVideoAsync(vids[i].Id).Result.Title;
                            
                                ConsoleSender.SendCommand($"{title} was added to the queue",
                                    ConsoleSender.Command.Chat);
                         
                                Instance.BackgroundEnqueue(Instance.SongType.AAC, ext,
                                    executor.Name, title);
                                break;
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