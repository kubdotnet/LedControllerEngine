using System;
using System.Collections.Generic;
using System.Linq;

namespace LedControllerEngine.Assets.Effects
{
    public class SplitSidesSettingsModel
    {
        public int? WestHue { get; set; }
        public int? EastHue { get; set; }
        public int? FanPhaseOffset { get; set; }
        public int? SidePhaseOffset { get; set; }
        public KeyValuePair<SplitSidesPulseValues, string> Pulse { get; set; }
        public IEnumerable<KeyValuePair<SplitSidesPulseValues, string>> PulseOptions { get; internal set; }
        public int? PulseSpeed { get; set; }

        public SplitSidesSettingsModel()
        {
            PulseOptions = Enum.GetValues(typeof(SplitSidesPulseValues))
                .Cast<SplitSidesPulseValues>()
                .Select(v => new KeyValuePair<SplitSidesPulseValues, string>(v, v.GetDescription()));
        }
    }
}
