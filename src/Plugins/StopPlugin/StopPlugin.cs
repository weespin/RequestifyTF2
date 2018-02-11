namespace OofPlugin
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using RequestifyTF2.Api;

    public class StopPlugin : IRequestifyPlugin
    {
        public string Author => "Weespin";

        public string Command => "!stop";

        public string Help => "Delete all background queue and stop current music!";

        public string Name => "Stop";

        public bool OnlyCode => false;

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