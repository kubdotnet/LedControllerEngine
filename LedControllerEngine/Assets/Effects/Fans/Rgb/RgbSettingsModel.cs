using Newtonsoft.Json;
using System.Windows.Media;

namespace LedControllerEngine.Assets.Effects
{
    public class RgbSettingsModel
    {
        [JsonProperty("color")]
        public Color Color { get; set; } = Colors.Fuchsia;
    }
}
