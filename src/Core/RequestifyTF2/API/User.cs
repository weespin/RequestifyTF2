using System;
using RequestifyTF2.API.Permission;

namespace RequestifyTF2.API
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
        public Group Group { get; set; } = Group.User;
    }
}