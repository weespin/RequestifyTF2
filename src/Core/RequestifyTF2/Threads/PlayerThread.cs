using System.Threading;
using System.Threading.Tasks;
using CSCore;
using CSCore.SoundOut;
using RequestifyTF2.API;

namespace RequestifyTF2.Threads
{
    
  internal static  class PlayerThread
  {

      private static Thread thread;

      public static void StopThread()
      {
          if (thread.IsAlive)
          {
              thread.Abort();
          }
      }
        public static void StartThread()
        {
            thread = new Thread(Play) {IsBackground = true};
            thread.Start();

            Logger.Write(Logger.Status.Info, Localization.Localization.CORE_STARTED_PLAYER_THREAD);
        }

        private static void Play()
        {
            while (true)
            {
                // BackGround
                if (Instance.SoundOutBackground.PlaybackState == PlaybackState.Playing)
                {
                    Instance.SoundOutBackground.Volume =
                        Instance.SoundOutForeground.PlaybackState == PlaybackState.Playing ? 0.25f : 0.5f;
                    if (Instance.SoundOutBackground.WaveSource != null)
                    {
                        if (Instance.SoundOutBackground.WaveSource.Length
                            - Instance.SoundOutBackground.WaveSource.Position
                            < Instance.SoundOutBackground.WaveSource.WaveFormat.BytesPerSecond / 100)
                        {
                            Instance.SoundOutBackground.Stop();
                        }
                    }
                }

                if (Instance.BackGroundQueue.GetQueueLenght() > 0)
                {
                    if (Instance.SoundOutBackground.PlaybackState == PlaybackState.Stopped)
                    {
                        Instance.Song s;
                        if (Instance.BackGroundQueue.PlayList.TryDequeue(out s))
                        {
                            Task.Run(
                                () =>
                                {
                                    ConsoleSender.SendCommand(
                                        string.Format(Localization.Localization.CORE_PLAYING_TITLE_FROM, s.Title, s.RequestedBy),
                                        ConsoleSender.Command.Chat);
                                    Player(s.Source, Instance.SoundOutBackground);
                                    Instance.SoundOutBackground.Volume = 0.10f;
                                });
                        }
                    }
                }

                // First Placed!
                if (Instance.SoundOutForeground.PlaybackState == PlaybackState.Playing)
                {
                    if (Instance.SoundOutForeground.WaveSource != null)
                    {
                        if (Instance.SoundOutForeground.WaveSource.Length
                            - Instance.SoundOutForeground.WaveSource.Position
                            < Instance.SoundOutForeground.WaveSource.WaveFormat.BytesPerSecond / 100)
                        {
                            Instance.SoundOutForeground.Stop();
                        }
                    }
                }

                if (Instance.QueueForeGround.Count > 0)
                {
                    if (Instance.SoundOutForeground.PlaybackState == PlaybackState.Stopped)
                    {
                        IWaveSource s;
                        if (Instance.QueueForeGround.TryDequeue(out s))
                        {
                            Task.Run(() => { Player(s, Instance.SoundOutForeground); });
                        }
                    }
                }

                Thread.Sleep(60);
            }
        }

        private static Task Player(IWaveSource decoder, WasapiOut device)
        {
            if (device.PlaybackState != PlaybackState.Stopped)
            {
                device.Stop();
            }

            device.Initialize(decoder);
            device.Play();
            return Task.FromResult<object>(null);
        }
    }
}