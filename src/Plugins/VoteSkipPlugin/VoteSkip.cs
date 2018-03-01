namespace VoteSkipPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using CSCore.SoundOut;

    using RequestifyTF2.Api;
    public class VotePlugin : IRequestifyPlugin
    {
        public string Author => "Weespin";
        public string Name => "Vote for skip!";
        public string Desc => "!voteskip";
    }
    public class VoteCommand : IRequestifyCommand
    {
        private long MusicId;

        private int PlayersCount;

        private readonly List<string> VoteUsers = new List<string>();


        public string Help => "Vote for skip!";
        public string Name => "voteskip";
     

        public bool OnlyAdmin => false;
        public List<string> Alias => new List<string>();

        public void Execute(string executor, List<string> arguments)
        {
            ConsoleSender.SendCommand("status",ConsoleSender.Command.Raw);
            if (Instance.SoundOutBackground.PlaybackState == PlaybackState.Playing)
            {
                if (this.MusicId != Instance.SoundOutBackground.WaveSource.Length)
                {
                    this.VoteUsers.Clear();
                    this.MusicId = Instance.SoundOutBackground.WaveSource.Length;
                    this.VoteUsers.Add(executor);
                    ConsoleSender.SendCommand(
                           $"{executor} voted to skip this song. {this.VoteUsers.Count}/{this.PlayersCount}",
                           ConsoleSender.Command.Chat);
                    if (this.VoteUsers.Count >= this.PlayersCount / 2)
                    {
                        Instance.SoundOutBackground.Stop();
                        ConsoleSender.SendCommand($"This song has been skipped", ConsoleSender.Command.Chat);
                    }
                    return;
                }
                else
                {
                    if (this.VoteUsers.Count >= this.PlayersCount / 2)
                    {
                        Instance.SoundOutBackground.Stop();
                        ConsoleSender.SendCommand($"This song has been skipped", ConsoleSender.Command.Chat);
                    }
                    if (this.VoteUsers.Contains(executor))
                    {
                        ConsoleSender.SendCommand(
                            $"{executor} already voted to skip this song. {this.VoteUsers.Count}/{this.PlayersCount}",
                            ConsoleSender.Command.Chat);

                    }
                    else
                    {
                        ConsoleSender.SendCommand(
                            $"{executor} voted to skip this song. {this.VoteUsers.Count}/{this.PlayersCount}",
                            ConsoleSender.Command.Chat);
                        if ( this.VoteUsers.Count>= this.PlayersCount)
                        {
                            Instance.SoundOutBackground.Stop();
                            ConsoleSender.SendCommand($"This song has been skipped", ConsoleSender.Command.Chat);
                        }
                    }
                }
            }
            else
            {
                ConsoleSender.SendCommand($"{executor}, the queue is empty.",ConsoleSender.Command.Chat);
            }
        }

        public void OnLoad()
        {
            Events.UndefinedMessage.OnUndefinedMessage += this.OnUndef;
        }

        private void OnUndef(Events.UndefinedMessageArgs e)
        {
            var reg = new Regex(@"players : (\d+) humans, (\d+) bots \((\d+) max\)").Match(e.Message);
            if (reg.Success)
            {
                this.PlayersCount = int.Parse(reg.Groups[1].Value);
            }
        }
    }
}