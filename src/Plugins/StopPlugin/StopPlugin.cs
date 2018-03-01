namespace OofPlugin
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using RequestifyTF2.Api;

    public class StopPlugin : IRequestifyPlugin
    {
        public string Author => "Weespin";
        public string Name => "Requestify";
        public string Desc => "!stop";
    }
    public class StopCommand : IRequestifyCommand
    {
        public string Author => "Weespin";

        public string Command => "!stop";

        public string Help => "Delete all background queue and stop current music!";

        public string Name => "stop";

        public List<string> Alias => new List<string>();
       

        public bool OnlyAdmin => false;

        public void Execute(string executor, List<string> arguments)
        {
            if (executor != Instance.Config.Admin)
            {
                return;
            }

            if (arguments[0] == "cur")
            {
                Instance.SoundOutBackground.Stop();
                return;
            }

            if (arguments[0] == "que")
            {
                Instance.BackGroundQueue.PlayList = new ConcurrentQueue<Instance.Song>();
                return;
            }

            Instance.SoundOutBackground.Stop();
            Instance.BackGroundQueue.PlayList = new ConcurrentQueue<Instance.Song>();
        }
    }
}