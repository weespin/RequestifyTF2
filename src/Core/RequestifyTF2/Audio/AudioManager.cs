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

using System;
using System.Collections.Concurrent;
using System.Linq;
using CSCore;
using CSCore.Codecs.AAC;
using CSCore.Codecs.MP3;
using CSCore.SoundOut;
using RequestifyTF2.API;
using RequestifyTF2.Audio.Utils;

namespace RequestifyTF2.Audio
{
    public static class AudioManager
    {
        public static class Extra
        {
            /// <summary>
            ///     Extra channel. Use it for very fast sounds. Does not have a queue!
            /// </summary>
            public static WasapiOut SoundOut { get; set; } = new WasapiOut();
        }

        public static class BackGround
        {
            /// <summary>
            ///     Background channel. Good for long sounds and music.
            /// </summary>
            public static WasapiOut SoundOut { get; set; } = new WasapiOut();

            public static bool AddEqueue(SongType songtype, string Link, string RequestedBy, string title)
            {
                try
                {
                    PlayList.Enqueue(songtype == SongType.MP3
                        ? new Song(title, new Mp3MediafoundationDecoder(Link), new User {Name = RequestedBy, Tag = 0})
                        : new Song(title, new AacDecoder(Link), new User {Name = RequestedBy, Tag = 0}));

                    return true;
                }
                catch (Exception e)
                {
                    Logger.Nlogger.Error(e);
                    return false;
                }
            }

            public static ConcurrentQueue<Song> PlayList { get; set; } = new ConcurrentQueue<Song>();

            public static void AddSong(Song song)
            {
                PlayList.Enqueue(song);
            }

            public static Song GetFarSong()
            {
                return PlayList.Last();
            }

            public static Song GetNearestSong()
            {
                return PlayList.First();
            }

            public static int GetQueueCount()
            {
                return PlayList.Count;
            }
        }

        public static class ForeGround
        {
            /// <summary>
            ///     Foreground channel. When sound is playing from this channel, the background channel will be silent.
            /// </summary>
            public static WasapiOut SoundOut { get; set; } = new WasapiOut();

            public static ConcurrentQueue<IWaveSource> PlayList { get; set; } = new ConcurrentQueue<IWaveSource>();

            public static void AddSong(IWaveSource song)
            {
                PlayList.Enqueue(song);
            }

            public static IWaveSource GetFarSong()
            {
                return PlayList.Last();
            }

            /// <summary>
            ///     Don't use it for playing music. It will show nearest songs only.
            /// </summary>
            /// <returns>
            ///     Return Nearest in Queue song. <see cref="IWaveSource" />.
            /// </returns>
            public static IWaveSource GetNearestSong()
            {
                return PlayList.First();
            }

            public static int GetQueueLenght()
            {
                return PlayList.Count;
            }
        }

        public static void Init()
        {
        }

        static AudioManager()
        {
            DeviceChanger.AutoSetDevice();
        }

        public class Song
        {
            public bool Dequeued { get; set; }

            public User RequestedBy { get; set; }

            public IWaveSource Source { get; set; }

            public string Title { get; set; }

            public Song(string title, IWaveSource source, User executor)
            {
                Title = title;
                Source = source;
                RequestedBy = executor;
            }
        }

        public enum SongType
        {
            MP3,
            AAC
        }
    }
}