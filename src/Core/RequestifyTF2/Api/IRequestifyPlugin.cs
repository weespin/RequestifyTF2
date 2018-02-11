namespace RequestifyTF2.Api
{
    using System.Collections.Generic;

    public interface IRequestifyPlugin
    {
        string Author { get; }

        string Command { get; }

        string Help { get; }

        string Name { get; }

        bool OnlyCode { get; }

        void Execute(string executor, List<string> arguments);
    }
}