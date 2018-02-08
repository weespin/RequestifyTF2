using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.Codecs;
using CSCore.Codecs.AAC;
using CSCore.CoreAudioAPI;
using CSCore.Ffmpeg;
using CSCore.SoundOut;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace ConsoleApplication1
{
    class Program
    {
    
        static void Main(string[] args)
        {
            List<MMDevice> _devices = new List<MMDevice>();
            var url = "https://youtu.be/AZKcl4-tcuo";
            var id = YoutubeClient.ParseVideoId(url); // "bnsUkE8i0tU"
            var client = new YoutubeClient();
            var streamInfoSet = client.GetVideoMediaStreamInfosAsync(id);
            var streamInfo = streamInfoSet.Result.Audio
                .FirstOrDefault(n => n.AudioEncoding == AudioEncoding.Aac || n.AudioEncoding == AudioEncoding.Mp3);
            if (streamInfo == null)
            {
                return;
            }

            var ext = streamInfo.Url;
           var s= new FfmpegDecoder(ext);
            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            {
                using (
                    var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    foreach (var device in mmdeviceCollection)
                    {
                        _devices.Add(device);
                    }
                }
            }

            var _soundOut = new WasapiOut() { Latency = 100, Device = _devices.Where(n=>n.FriendlyName.Contains("Virtual")).FirstOrDefault() };
            _soundOut.Initialize(s);
            _soundOut.Play();
        
            Console.Read();
        }
    }
}
