using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OofPlugin
{
    using System.Collections.Concurrent;
    using System.IO;

    using CSCore;

    using RequestifyTF2.Api;

    public class StopPlugin : IRequestifyPlugin
    {
        public string Name => "Stop";

        public string Author => "Weespin";

        public string Command => "!stop";

        public string Help => "Delete all background queue and stop current music!";

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
                Instance.QueueBackGround = new ConcurrentQueue<IWaveSource>();
                return;
            }
            Instance.SoundOutBackground.Stop();
            Instance.QueueBackGround = new ConcurrentQueue<IWaveSource>();

        }
    }
}
