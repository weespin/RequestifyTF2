using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CSCore.SoundOut;
using Newtonsoft.Json;
using RequestifyTF2.API;
using RequestifyTF2.PluginLoader;
using YoutubeExplode;
using YoutubeExplode.Search;

namespace RequestPlugin
{

    public class RequestPlugin : IRequestifyPlugin
    {
        public void OnLoad()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            LoadLibrary("YoutubeExplode");
            LoadLibrary("LtGt");
            LoadLibrary("Sprache");
            Events.UndefinedMessage.OnUndefinedMessage += OnUndef;
        }
        static WebClient web = new WebClient();

        public static RequestPlugin instance;
        public string Author => "Weespin";
        public string Name => "Requestify";
        public string Desc => "!request \"link\"";

        
        private void OnUndef(Events.UndefinedMessageArgs e)
        {
	        var reg = new Regex(@"players : (\d+) humans, (\d+) bots \((\d+) max\)").Match(e.Message);
	        if (reg.Success)
	        {
		        Data.PlayersCount = int.Parse(reg.Groups[1].Value);
	        }
        }
        private bool LoadLibrary(string libname)
        {
	        if (!File.Exists($"./lib/{libname}.dll"))
	        {
		        web.DownloadFile(
			        $"https://github.com/weespin/reqdeps/blob/master/{libname}.dll?raw=true",
			        $"./lib/{libname}.dll");
                if (Libraries.LoadFile($"./lib/{libname}.dll") == false)
                {
                    Logger.Write(Logger.Status.Error, $"Failed to load {libname}.dll");
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
    }
    public class AsyncEnumerableExtensions
    {
	    public static async Task<List<T>> ToListAsync<T>( IAsyncEnumerable<T> items,
		    CancellationToken cancellationToken = default)
	    {
		    var results = new List<T>();
            await foreach (var item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (results.Count > 12)
                {
                    break;
                }
                results.Add(item);
                
            }

            return results;
	    }
    }


    static class Data
    {
	    public static int PlayersCount = 0;

    }
    public class VoteCommand : IRequestifyCommand
    {

        public string Help => "Vote for skip!";
        public string Name => "voteskip";
        public readonly List<string> VoteUsers = new List<string>();
        public  static long MusicId;

        public bool OnlyAdmin => false;
        public List<string> Alias => new List<string>();
        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
     
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

                    var pl = Data.PlayersCount < 4 ?  1 : Data.PlayersCount / 2 ;
                    ConsoleSender.SendCommand(
                            $"{executor.Name} voted to skip this song. { VoteUsers.Count}/{pl}",
                            ConsoleSender.Command.Chat);
                    

                    if (VoteUsers.Count >= Data.PlayersCount / 4)
                    {
                        Instance.SoundOutBackground.Stop();
                       
                            ConsoleSender.SendCommand($"This song has been skipped", ConsoleSender.Command.Chat);
                        
                    }
                }
                else
                {
                    if (VoteUsers.Count >= Data.PlayersCount / 4)
                    {
                        Instance.SoundOutBackground.Stop();
                       
                            ConsoleSender.SendCommand($"This song has been skipped", ConsoleSender.Command.Chat);
                        
                    }

                    if (VoteUsers.Contains(executor.Name))
                    {
                        
                            ConsoleSender.SendCommand(
                                $"{executor.Name} already voted to skip this song. { VoteUsers.Count}/{Data.PlayersCount}",
                                ConsoleSender.Command.Chat);
                        
                    }
                    else
                    {
                        ConsoleSender.SendCommand(
                            $"{executor.Name} voted to skip this song. {VoteUsers.Count}/{Data.PlayersCount}",
                            ConsoleSender.Command.Chat);
                        if (VoteUsers.Count >= Data.PlayersCount)
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
                        Instance.BackGroundQueue.PlayList = new ConcurrentQueue<Instance.Media>();
                        ConsoleSender.SendCommand("Done, now playlist is clear", ConsoleSender.Command.Chat);
                        return;
                    }

                }

                Instance.SoundOutBackground.Stop();
                Instance.BackGroundQueue.PlayList = new ConcurrentQueue<Instance.Media>();
                ConsoleSender.SendCommand("BackGroundQueue is now clear", ConsoleSender.Command.Chat);
            }
        }

        public class RequestCommand : IRequestifyCommand
        {
            private static readonly int AppID = 1553518929;

            private static readonly string clientID = "NxDq1GKZ5tLDRohQGfJ7lYVKiephsF3G";
            private static string soundCloudResolveEndpoint { get; } = "https://api-v2.SoundCloud.com/resolve";
            public string Help => "Play music. Supports soundcloud and youtube!";

            public string Name => "request";
            public List<string> Alias => new List<string>();

            public bool OnlyAdmin => false;
            public class TrackInfo
            {
                public string title { get; set; }
                public Media media { get; set; }
            }

            public class Transcoding
            {
                public string url { get; set; }
                public string preset { get; set; }
            }

            public class Media
            {
                public List<Transcoding> transcodings { get; set; }
            }
            public class TrackMediaData
            {
                public string url { get; set; }
            }
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
                            string clientId = "";
                            var HTMLFile = web.DownloadString(url);
                            var JSRegex = new Regex("<script(?:.*)src=\"(https\\:\\/\\/a-v2.*)\"");
                            var JSMatches = JSRegex.Matches(HTMLFile);
                            for (int i = JSMatches.Count - 1; i >= 0; i--)
                            {
                                var JSFile = web.DownloadString(JSMatches[i].Groups[1].Value);
                                var clientregex = new Regex("client_id:\"(.*?)\",").Match(JSFile);
                                if (clientregex.Success)
                                {
                                    clientId = clientregex.Groups[1].Value;
                                    break;
                                }
                            }

                            var TrackInfoURL = soundCloudResolveEndpoint + $"?url={url}&client_id={clientId}";
                            var TrachInfoHTML = web.DownloadString(TrackInfoURL);
                            var TrackInfoData = JsonConvert.DeserializeObject<TrackInfo>(TrachInfoHTML);
                            var TrackMediaURL = "";
                            foreach (var transcoding in TrackInfoData.media.transcodings)
                            {
                                if (transcoding.preset.Contains("mp3"))
                                {
                                    var TrackMediaDataString = web.DownloadString(transcoding.url + $"?client_id={clientId}");
                                    TrackMediaURL = JsonConvert.DeserializeObject<TrackMediaData>(TrackMediaDataString).url;
                                    break;
                                }
                            }
                            var TrackPlayList = web.DownloadString(TrackMediaURL);
                            var TrackPlayListSplitted = TrackPlayList.Split('\n');
                            var MP3TrackURL = TrackPlayListSplitted[TrackPlayListSplitted.Length - 2];
                            var MP3TrackURLSplitted = MP3TrackURL.Split('/');
                            var FinalURLTrack = "";
                            for (var index = 0; index < MP3TrackURLSplitted.Length; index++)
                            {
                                var a = MP3TrackURLSplitted[index];
                                if (a == "media")
                                {
                                    MP3TrackURLSplitted[index + 1] = "0";
                                    FinalURLTrack = string.Join("/", MP3TrackURLSplitted);
                                    break;
                                }
                            }
                            Instance.BackgroundEnqueue(Instance.SongType.MP3, FinalURLTrack,
                                executor.Name, TrackInfoData.title);
                            return;
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

		            var streamInfo = streamManifest.GetAudioStreams().Where(n => n.AudioCodec.Contains("mp4"))
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
		            var videos = AsyncEnumerableExtensions.ToListAsync<ISearchResult>(client.Search.GetVideosAsync(text)).Result;

		            if (videos.Count > 0)
                    {
                        foreach (var video in videos)
                        {
                            var streamManifest = client.Videos.Streams.GetManifestAsync(video.Url).Result;
                            if (streamManifest.Streams.Count == 0)
                            {
                                return;
                            }

                            var streamInfo = streamManifest.GetAudioStreams().Where(n => n.Container.Name.Contains("mp4"))
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

                            var title = client.Videos.GetAsync(video.Url).Result.Title;
                            Instance.BackgroundEnqueue(Instance.SongType.AAC, ext,
                                executor.Name, title);
                            break;
                        }
                    }

	            }
            }
        }
    }
}