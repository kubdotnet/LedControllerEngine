using Newtonsoft.Json;

namespace LedControllerEngine.Assets.Effects.Stripes
{
    public class SingleColorPulseSettingsModel
    {
        [JsonProperty("hue")]
        public int? Hue { get; set; }

        [JsonProperty("speed")]
        public int? Speed { get; set; }
    }
}
