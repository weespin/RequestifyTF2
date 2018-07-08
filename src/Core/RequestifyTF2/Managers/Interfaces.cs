using System.Collections.Generic;

namespace RequestifyTF2.Api
{
    public interface IRequestifyPlugin
    {
        string Author { get; }
        string Name { get; }
        string Desc { get; }
    }

    public interface IRequestifyCommand
    {
        string Help { get; }
        string Name { get; }
        bool OnlyAdmin { get; }
        List<string> Alias { get; }
        void Execute(User executor, List<string> arguments);
    }
}