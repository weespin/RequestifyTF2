using Newtonsoft.Json;

namespace RequestifyTF2Forms.JsonDataClasses
{
    internal class ConfigJsonData
    {
        [JsonProperty("Admin")] public string Admin { get; set; }

        [JsonProperty("GameDirectory")] public string GameDirectory { get; set; }
    }
}