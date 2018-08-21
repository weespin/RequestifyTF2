using System.Collections.Generic;
using RequestifyTF2.API.Permission;

namespace RequestifyTF2.API
{
    public interface IRequestifyPlugin
    {
        string Author { get; }
        string Name { get; }
        string Desc { get; }
    }

    public interface IRequestifyCommand
    {
        Rules Permission { get; }
        string Help { get; }
        string Name { get; }
        List<string> Alias { get; }
        void Execute(User executor, List<string> arguments);
    }
}