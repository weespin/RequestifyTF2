using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using CSCore;
using CSCore.Codecs.AAC;
using CSCore.Codecs.MP3;
using CSCore.SoundOut;
using Newtonsoft.Json;
using RequestifyTF2.API;
using RequestifyTF2.PluginLoader;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

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
        static WebClient web = new WebClient();
        public void OnLoad()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            LoadLibrary("YoutubeExplode");
            LoadLibrary("LtGt");
            LoadLibrary("Sprache");

            Events.UndefinedMessage.OnUndefinedMessage += OnUndef;
        }
        private void LoadLibrary(string libname)
        {
            if (!File.Exists($"./lib/{libname}.dll"))
            {
                web.DownloadFile(
                    $"https://github.com/weespin/reqdeps/blob/master/{libname}.dll?raw=true",
                    $"./lib/{libname}.dll");
                Libraries.LoadFile($"./lib/{libname}.dll");
            }
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
		            var client = new YoutubeClient();
		            var video = client.Videos.GetAsync(url).Result;
		            var streamManifest = client.Videos.Streams.GetManifestAsync(video.Id).Result;
		            if (streamManifest.Streams.Count == 0)
		            {
			            return;
		            }

		            var streamInfo = streamManifest.GetAudioOnly().Where(n => n.AudioCodec.Contains("mp4"))
			            .FirstOrDefault();

		            if (streamInfo == null)
		            {
			            return;
		            }

		            var ext = streamInfo.Url;
		            if (ext == string.Empty)
		            {
			            return;
		            }

		            var title = video.Title;
		            ConsoleSender.SendCommand($"{title} was added to the queue", ConsoleSender.Command.Chat);

		            Instance.BackgroundEnqueue(Instance.SongType.AAC, ext,
			            executor.Name, title);

	            }
	            else
	            {
		            var text = arguments.Aggregate(" ", (current, argument) => current + " " + argument);
		            var client = new YoutubeClient();
		            var vids = client.Search.GetVideosAsync(text).BufferAsync(5).Result;

		            if (vids.Count > 0)
		            {
			            for (int i = 0; i < vids.Count; i++)
			            {
				            var streamManifest = client.Videos.Streams.GetManifestAsync(vids[i].Id).Result;
				            if (streamManifest.Streams.Count == 0)
				            {
					            return;
				            }

				            var streamInfo = streamManifest.GetAudioOnly().Where(n => n.AudioCodec.Contains("mp4"))
					            .FirstOrDefault();

				            if (streamInfo == null)
				            {
					            return;
				            }

				            var ext = streamInfo.Url;
				            if (ext == string.Empty)
				            {
					            return;
				            }

				            var title = client.Videos.GetAsync(vids[i].Id).Result.Title;
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