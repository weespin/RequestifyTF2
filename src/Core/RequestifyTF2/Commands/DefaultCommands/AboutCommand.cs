using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RequestifyTF2.API;

namespace RequestifyTF2.Commands
{
    class AboutCommand : IRequestifyCommand
    {
        public string Help => "About command";
        public string Name => "about";
        public bool OnlyAdmin => false;
        public List<string> Alias => new List<string>();
        public void Execute(User executor, List<string> arguments)
        {
            ConsoleSender.SendCommand("RequestifyTF2 by DllMain/nullptr/Weespin",ConsoleSender.Command.Chat);
            ConsoleSender.SendCommand("Download at: github.com/weespin/RequestifyTF2", ConsoleSender.Command.Chat);
        }
    }
}
