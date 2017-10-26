using System;
using System.Collections.Generic;
using System.Linq;

namespace LedControllerEngine.Assets.Effects
{
    public class SplitQuartersSettingsModel
    {
        public int? NorthWestHue { get; set; }
        public int? NorthEastHue { get; set; }
        public int? SouthEastHue { get; set; }
        public int? SouthWestHue { get; set; }
        public int? PulsePhaseOffset { get; set; }
        public KeyValuePair<SplitSidesPulseValues, string> Pulse { get; set; }
        public IEnumerable<KeyValuePair<SplitSidesPulseValues, string>> PulseOptions { get; internal set; }
        public int? PulseSpeed { get; set; }

        public SplitQuartersSettingsModel()
        {
            PulseOptions = Enum.GetValues(typeof(SplitSidesPulseValues))
                .Cast<SplitSidesPulseValues>()
                .Select(v => new KeyValuePair<SplitSidesPulseValues, string>(v, v.GetDescription()));
        }
    }
}
