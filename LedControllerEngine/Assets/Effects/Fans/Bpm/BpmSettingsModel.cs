using Newtonsoft.Json;

namespace LedControllerEngine.Assets.Effects
{
    public class BpmSettingsModel
    {
        [JsonProperty("bpm")]
        public int? Bpm { get; set; }

        [JsonProperty("hue")]
        public int? Hue { get; set; }

        [JsonProperty("beat")]
        public int? Beat { get; set; }
    }
}
