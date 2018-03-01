namespace RequestifyTF2.Api
{
    using System.Collections.Generic;
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
        void Execute(string executor, List<string> arguments);
    }

 
}