using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSCore;
using CSCore.Ffmpeg;
using CSCore.SoundOut;
using RequestifyTF2.Api;

namespace RequestifyTF2.Threads
{
    class PlayerThread
    {
        static object Locker = new object();
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
                if (Instance.SoundOutBackGround.PlaybackState == PlaybackState.Playing)
                {
                    Instance.SoundOutBackGround.Volume = Instance.SoundOutForeGround.PlaybackState == PlaybackState.Playing ? 0.65f : 1f;
                    if (Instance.SoundOutBackGround.WaveSource != null)
                    {
                        if ((Instance.SoundOutBackGround.WaveSource.Length - Instance.SoundOutBackGround.WaveSource.Position) <
                            Instance.SoundOutBackGround.WaveSource.WaveFormat.BytesPerSecond / 100)
                        { 
                            //Little hack becouse ffmpeg don't stop playing :(
                            Instance.SoundOutBackGround.Stop();
                        }
                    }
                }
                if (Instance.QueueBackGround.Count > 0)
                {
                   
                  //Debug.WriteLine(Instance.SoundOut.WaveSource.GetLength()+"/"+Instance.SoundOut.WaveSource.GetPosition());
                    if (Instance.SoundOutBackGround.PlaybackState == PlaybackState.Stopped)
                    {

                        IWaveSource s;
                            if (Instance.QueueBackGround.TryDequeue(out s))
                            {

                                //   Instance.SoundOut.Dispose();
                                Task.Run(() => { Player(s,Instance.SoundOutBackGround); });         
                            }
                        }
                    }
                //First Placed!
                if (Instance.SoundOutForeGround.PlaybackState == PlaybackState.Playing)
                {
                    if (Instance.SoundOutForeGround.WaveSource != null)
                    {
                        if ((Instance.SoundOutForeGround.WaveSource.Length - Instance.SoundOutForeGround.WaveSource.Position) <
                            Instance.SoundOutForeGround.WaveSource.WaveFormat.BytesPerSecond / 100)
                        {
                            //Little hack becouse ffmpeg don't stop playing :(
                            Instance.SoundOutForeGround.Stop();
                        }
                    }
                }
                if (Instance.QueueForerground.Count > 0)
                {

                    //Debug.WriteLine(Instance.SoundOut.WaveSource.GetLength()+"/"+Instance.SoundOut.WaveSource.GetPosition());
                    if (Instance.SoundOutForeGround.PlaybackState == PlaybackState.Stopped)
                    {

                        IWaveSource s;
                        if (Instance.QueueForerground.TryDequeue(out s))
                        {

                            //   Instance.SoundOut.Dispose();
                            Task.Run(() => { Player(s, Instance.SoundOutForeGround); });
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
