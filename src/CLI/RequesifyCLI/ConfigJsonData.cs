namespace RequesifyCLI
{
    using Newtonsoft.Json;

    internal class ConfigJsonData
    {
        [JsonProperty("Admin")]
        public string Admin { get; set; }

        [JsonProperty("GameDirectory")]
        public string GameDirectory { get; set; }

        [JsonProperty("OnlyWithCode")]
        public bool OnlyWithCode { get; set; }
    }
}