using System;

namespace RequestifyTF2.Api
{
    [Flags]
    public enum Tag
    {
        Undefined = 1,
        Spectator = 2,
        Team = 4,
        Dead = 8
    }
    public class User
    {
        public Tag Tag = Tag.Undefined;
        public string Name = "";
    }
}