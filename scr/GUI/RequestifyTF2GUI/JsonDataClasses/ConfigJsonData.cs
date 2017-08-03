using Newtonsoft.Json;

namespace RequestifyTF2Forms.JsonDataClasses
{
    internal class ConfigJsonData
    {
        [JsonProperty("GameDirectory")]
        public string GameDirectory { get; set; }

        [JsonProperty("OnlyWithCode")]
        public bool OnlyWithCode { get; set; }
    }
}