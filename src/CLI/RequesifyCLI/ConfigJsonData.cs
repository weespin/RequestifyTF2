using Newtonsoft.Json;

namespace RequesifyCLI
{
    internal class ConfigJsonData
    {
        [JsonProperty("GameDirectory")]
        public string GameDirectory { get; set; }

        [JsonProperty("OnlyWithCode")]
        public bool OnlyWithCode { get; set; }
        [JsonProperty("Admin")]
        public string Admin { get; set; }
    }
    
}
