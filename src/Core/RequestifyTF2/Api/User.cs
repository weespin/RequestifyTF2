namespace RequestifyTF2.Api
{
    public enum Tag
    {
        Spec,
        Team,
        Und
    }
    public class User
    {
        public bool Dead = false;
        public Tag Tag = Tag.Und;
        public string Name = "";
    }
}