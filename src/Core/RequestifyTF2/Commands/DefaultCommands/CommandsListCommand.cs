using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequestifyTF2.API;
using RequestifyTF2.Managers;

namespace RequestifyTF2.Commands.DefaultCommands
{
    class CommandsListCommand : IRequestifyCommand
    {
        public string Help => "A command to get all possible commands";
        public string Name => "commands";
        public bool OnlyAdmin => true;
        public List<string> Alias => new List<string>();

        public void Execute(User executor, List<string> arguments)
        {
            var possiblecommands = Instance.Commands.GetCommands().Where(n => n.Status == CommandManager.Status.Enabled)
                .ToList();
            if (possiblecommands.Count > 0)
            {
                if (arguments.Count > 0)
                {
                    ushort page;
                    if (ushort.TryParse(arguments[0], out page))
                    {
                        if (page <= possiblecommands.Count-1&&page!=0)
                        {
                            ConsoleSender.SendCommand($"-> !{possiblecommands[page-1].Name} | {possiblecommands[page-1].Help}",ConsoleSender.Command.Chat);
                            ConsoleSender.SendCommand($"Page {page} / {possiblecommands.Count-1}",ConsoleSender.Command.Chat);
                        }
                        else
                        {
                            ConsoleSender.SendCommand($"Page is out of range",
                                ConsoleSender.Command.Chat);
                        }
                    }
                    else
                    {
                        ConsoleSender.SendCommand($"Can't parse page number",
                            ConsoleSender.Command.Chat);
                    }
                }
                else
                {
                    ConsoleSender.SendCommand($"There are {possiblecommands.Count-1} possible commands to display",
                        ConsoleSender.Command.Chat);
                    ConsoleSender.SendCommand($"Use !{this.Name} {{page}} to display commands",ConsoleSender.Command.Chat);
                }
                

            }
            else
            {
                ConsoleSender.SendCommand("0 possible commands to display",ConsoleSender.Command.Chat);
            }

        }
    }
}
