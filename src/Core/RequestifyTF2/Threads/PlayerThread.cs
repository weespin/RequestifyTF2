// RequestifyTF2
// Copyright (C) 2018  Villiam Nmerukini
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System.Threading;
using System.Threading.Tasks;
using CSCore;
using CSCore.SoundOut;
using RequestifyTF2.API;
using RequestifyTF2.API.ConsoleAPI;
using RequestifyTF2.Audio;

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
            AudioManager.Init();
            thread = new Thread(Play) {IsBackground = true};
            thread.Start();

            Logger.Nlogger.Debug(Localization.Localization.CORE_STARTED_PLAYER_THREAD);
            
        }

        private static void Play()
        {
            while (true)
            {
                // BackGround
                if (AudioManager.BackGround.SoundOut.PlaybackState == PlaybackState.Playing)
                {
                    AudioManager.BackGround.SoundOut.Volume =
                        AudioManager.ForeGround.SoundOut.PlaybackState == PlaybackState.Playing ? 0.25f : 0.5f;
                    if (AudioManager.BackGround.SoundOut.WaveSource != null)
                    {
                        if (AudioManager.BackGround.SoundOut.WaveSource.Length
                            - AudioManager.BackGround.SoundOut.WaveSource.Position
                            < AudioManager.BackGround.SoundOut.WaveSource.WaveFormat.BytesPerSecond / 100)
                        {
                            AudioManager.BackGround.SoundOut.Stop();
                        }
                    }
                }

                if (AudioManager.BackGround.GetQueueCount() > 0)
                {
                    if (AudioManager.BackGround.SoundOut.PlaybackState == PlaybackState.Stopped)
                    {
                       AudioManager.Song s;
                        if (AudioManager.BackGround.PlayList.TryDequeue(out s))
                        {
                            Task.Run(
                                () =>
                                {
                                    ConsoleSender.SendCommand(
                                        string.Format(Localization.Localization.CORE_PLAYING_TITLE_FROM, s.Title, s.RequestedBy.Name),
                                        ConsoleSender.Command.Chat);
                                    Player(s.Source, AudioManager.BackGround.SoundOut);
                                   AudioManager.BackGround.SoundOut.Volume = 0.10f;
                                });
                        }
                    }
                }

                // First Placed!
                if (AudioManager.ForeGround.SoundOut.PlaybackState == PlaybackState.Playing)
                {
                    if (AudioManager.ForeGround.SoundOut.WaveSource != null)
                    {
                        if (AudioManager.ForeGround.SoundOut.WaveSource.Length
                            - AudioManager.ForeGround.SoundOut.WaveSource.Position
                            < AudioManager.ForeGround.SoundOut.WaveSource.WaveFormat.BytesPerSecond / 100)
                        {
                            AudioManager.ForeGround.SoundOut.Stop();
                        }
                    }
                }

                if (AudioManager.ForeGround.PlayList.Count > 0)
                {
                    if (AudioManager.ForeGround.SoundOut.PlaybackState == PlaybackState.Stopped)
                    {
                        IWaveSource s;
                        if (AudioManager.ForeGround.PlayList.TryDequeue(out s))
                        {
                            Task.Run(() => { Player(s, AudioManager.ForeGround.SoundOut); });
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

            device.Initialize(decoder.ToMono()); //Mono > Stereo in micspams
            device.Play();
            return Task.FromResult<object>(null);
        }
    }
}