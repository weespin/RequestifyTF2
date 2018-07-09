using System;

namespace RequestifyTF2.Api
{
    [Flags]
    public enum Tag
    {
        None = 0,
        Spectator = 1 << 0,
        Team = 1 << 1,
        Dead = 1 << 2
    }

    public class User
    {
        public string Name { get; set; } = "";
        public Tag Tag { get; set; } = Tag.None;
    }
}