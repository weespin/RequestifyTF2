using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSCore;
using CSCore.SoundOut;
using RequestifyTF2.Api;

namespace RequestifyTF2.Threads
{
    class PlayerThread
    {
        //static object Locker = new object();
        public static void Starter()
        {
            var thread = new Thread(Play) { IsBackground = true };
            thread.Start();
          
            Logger.Write(Logger.Status.Info, "Started Player Thread!");
        }


       
        private static void Play()
        {
            while (true)
            {
              //BackGround
                if (Instance.SoundOutBackground.PlaybackState == PlaybackState.Playing)
                {
                    Instance.SoundOutBackground.Volume = Instance.SoundOutForeground.PlaybackState == PlaybackState.Playing ? 0.25f :0.5f;
                    if (Instance.SoundOutBackground.WaveSource != null)
                    {
                        if ((Instance.SoundOutBackground.WaveSource.Length - Instance.SoundOutBackground.WaveSource.Position) <
                            Instance.SoundOutBackground.WaveSource.WaveFormat.BytesPerSecond / 100)
                        { 
                            //Little hack becouse ffmpeg don't stop playing :(
                            Instance.SoundOutBackground.Stop();
                        }
                    }
                }
                if (Instance.BackGroundQueue.GetQueueLenght() > 0)
                {
                   
                  //Debug.WriteLine(Instance.SoundOut.WaveSource.GetLength()+"/"+Instance.SoundOut.WaveSource.GetPosition());
                    if (Instance.SoundOutBackground.PlaybackState == PlaybackState.Stopped)
                    {

                      Instance.Song s;
                            if (Instance.BackGroundQueue.PlayList.TryDequeue(out s))
                            {

                                //   Instance.SoundOut.Dispose();
                                Task.Run(
                                    () =>
                                        {
                                            ConsoleSender.SendCommand($"Playing {s.Title} from {s.RequestedBy}",ConsoleSender.Command.Chat);
                                            Player(s.Source,Instance.SoundOutBackground);
                                            Instance.SoundOutBackground.Volume = 0.10f;
                                        });         
                            }
                        }
                    }
                //First Placed!
                if (Instance.SoundOutForeground.PlaybackState == PlaybackState.Playing)
                {
                    if (Instance.SoundOutForeground.WaveSource != null)
                    {
                        if ((Instance.SoundOutForeground.WaveSource.Length - Instance.SoundOutForeground.WaveSource.Position) <
                            Instance.SoundOutForeground.WaveSource.WaveFormat.BytesPerSecond / 100)
                        {
                            //Little hack becouse ffmpeg don't stop playing :(
                            Instance.SoundOutForeground.Stop();
                        }
                    }
                }
                if (Instance.QueueForeGround.Count > 0)
                {

                    //Debug.WriteLine(Instance.SoundOut.WaveSource.GetLength()+"/"+Instance.SoundOut.WaveSource.GetPosition());
                    if (Instance.SoundOutForeground.PlaybackState == PlaybackState.Stopped)
                    {

                        IWaveSource s;
                        if (Instance.QueueForeGround.TryDequeue(out s))
                        {

                            //   Instance.SoundOut.Dispose();
                            Task.Run(() => { Player(s, Instance.SoundOutForeground); });
                        }
                    }
                }

                Thread.Sleep(60);
            }
           
        }

        

        private static Task Player(IWaveSource decoder,WasapiOut device)
        {
           device.Initialize(decoder);
           device.Play();
            return Task.FromResult<object>(null);
        }
    }
}
