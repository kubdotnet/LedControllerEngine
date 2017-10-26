using System.ComponentModel;

namespace LedControllerEngine.Assets.Effects
{
    public enum SplitSidesPulseValues
    {
        [Description("No pulse")]
        No = 0,

        [Description("Sine")]
        Sin = 1,

        [Description("Sawtooth In")]
        SawToothIn = 2,

        [Description("Sawtooth Out")]
        SawToothOut = 3,

        [Description("Triangle Wave")]
        Triangle = 4
    }
}
