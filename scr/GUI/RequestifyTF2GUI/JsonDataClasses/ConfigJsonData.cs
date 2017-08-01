using Newtonsoft.Json;

namespace RequestifyTF2Forms.JsonDataClasses
{
    internal class ConfigJsonData
    {
        [JsonProperty("GameDir")]
        public string GameDir { get; set; }

        [JsonProperty("OnlyAdmin")]
        public bool OnlyAdmin { get; set; }
    }
}