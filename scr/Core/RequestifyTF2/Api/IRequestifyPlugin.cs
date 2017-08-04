using System.Collections.Generic;

namespace RequestifyTF2.Api
{
    public interface IRequestifyPlugin
    {
        string Name { get; }
        string Command { get; }
        bool OnlyCode { get; }
        string Author { get; }
        string Help { get;  }
        void Execute(string executor, List<string> arguments);
    }
}