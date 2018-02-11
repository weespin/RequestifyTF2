namespace VoteSkipPlugin
{
    using System;
    using System.Collections.Generic;

    using CSCore.SoundOut;

    using RequestifyTF2.Api;

    public class VoteSkip : IRequestifyPlugin
    {
        private long MusicId;

        private int PlayersCount;

        private readonly List<string> VoteUsers = new List<string>();

        public string Author => "Weespin";

        public string Command => "!voteskip";

        public string Help => "Vote for skip!";

        public string Name => "voteskip";

        public bool OnlyCode => false;

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
                        ConsoleSender.SendCommand(
                            $"{executor} already voted to skip this song. {this.VoteUsers.Count}/5",
                            ConsoleSender.Command.Chat);
                    }
                    else
                    {
                        ConsoleSender.SendCommand(
                            $"{executor} voted to skip this song. {this.VoteUsers.Count}/5",
                            ConsoleSender.Command.Chat);
                        if (this.VoteUsers.Count >= 5)
                        {
                            Instance.SoundOutBackground.Stop();
                            ConsoleSender.SendCommand($"This song has been skipped", ConsoleSender.Command.Chat);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("The queue is empty.");
            }
        }

        public void OnLoad()
        {
            Events.UndefinedMessage.OnUndefinedMessage += this.OnUndef;
        }

        private void OnUndef(Events.UndefinedMessageArgs e)
        {
            if (e.Message.StartsWith("players :"))
            {
            }
        }
    }
}