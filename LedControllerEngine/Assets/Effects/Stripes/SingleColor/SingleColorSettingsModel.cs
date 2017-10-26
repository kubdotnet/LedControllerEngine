using Newtonsoft.Json;

namespace LedControllerEngine.Assets.Effects.Stripes
{
    public class SingleColorSettingsModel
    {
        [JsonProperty("hue")]
        public int? Hue { get; set; }
    }
}
