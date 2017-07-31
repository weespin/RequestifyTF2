namespace RequestifyTF2.Api
{
    public interface IPlugin
    {
        string Name { get; }
        string Command { get; }
        bool OnlyCode { get; }
        double Version { get; }
        string Author { get; }
        void Execute(string[] line, bool admin = false);
    }
}