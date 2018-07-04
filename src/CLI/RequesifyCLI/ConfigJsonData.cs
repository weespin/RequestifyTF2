using Newtonsoft.Json;

namespace RequesifyCLI
{
    internal class ConfigJsonData
    {
        [JsonProperty("Admin")] public string Admin { get; set; }

        [JsonProperty("GameDirectory")] public string GameDirectory { get; set; }
    }
}