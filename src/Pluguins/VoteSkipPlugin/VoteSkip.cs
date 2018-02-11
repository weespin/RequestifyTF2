using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteSkipPlugin
{
    using CSCore.SoundOut;

    using RequestifyTF2.Api;
    public class VoteSkip : IRequestifyPlugin
    {
        public string Name => "voteskip";

        public string Author => "Weespin";

        public string Command => "!voteskip";

        public string Help => "Vote for skip!";

        public bool OnlyCode => false;
        List<string> VoteUsers = new List<string>();

        private int PlayersCount;
        private long MusicId = 0;

        public void OnLoad()
        {
            Events.UndefinedString.OnUndefinedString += OnUndef;
        }

        private void OnUndef(Events.UndefinedStringArgs e)
        {
            if (e.Text.StartsWith("players :"))
            {

            }
        }


        public void Execute(string executor, List<string> arguments)
        {

            if (Instance.SoundOutBackground.PlaybackState == PlaybackState.Playing)
            {
                if (this.MusicId != Instance.SoundOutBackground.WaveSource.Length)
                {
                    this.VoteUsers.Clear();
                    this.MusicId = Instance.SoundOutBackground.WaveSource.Length;
                    this.VoteUsers.Add(executor);
                }
                else
                {

                    if (this.VoteUsers.Contains(executor))
                    {
                        RequestifyTF2.Api.ConsoleSender.SendCommand(
                            $"{executor} already voted to skip this song. {this.VoteUsers.Count}/5",
                            ConsoleSender.Command.Chat);
                        return;
                    }
                    else
                    {
                        RequestifyTF2.Api.ConsoleSender.SendCommand(
                            $"{executor} voted to skip this song. {this.VoteUsers.Count}/5",
                            ConsoleSender.Command.Chat);
                        if (this.VoteUsers.Count >= 5)
                        {
                            Instance.SoundOutBackground.Stop();
                            RequestifyTF2.Api.ConsoleSender.SendCommand(
                                $"This song has been skipped",
                                ConsoleSender.Command.Chat);
                        }
                    }

                }

            }
            else
            {
                Console.WriteLine("The queue is empty.");
            }
        }
    }
}
