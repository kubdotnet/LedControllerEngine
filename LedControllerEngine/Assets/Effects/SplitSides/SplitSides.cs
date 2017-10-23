using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets.Effects
{
    public class SplitSides : IEffect
    {
        public string Name { get => "Split Sides"; }
        public int ModeNumber { get => 7; }
        public Guid Id { get => new Guid("8FAB810D-A27C-401F-8D01-26A88EAA6612"); }
        public Type SettingsControl
        {
            get => typeof(SplitSidesSettings);
        }

        private SplitSidesSettingsModel _settingsModel = new SplitSidesSettingsModel();
        public object SettingsModel
        {
            get
            {
                return _settingsModel;
            }
            set
            {
                if (!(value is SingleSpinnerSettingsModel))
                {
                    throw new ArgumentException("Wrong type, SplitSidesSettingsModel expected");
                }

                _settingsModel = (SplitSidesSettingsModel)value;
            }
        }

        public Type EffectType => typeof(SplitSides);

        /// <summary>
        /// Gets the settings values.
        /// </summary>
        /// <returns>
        /// List of EffectSetting
        /// </returns>
        public List<EffectSetting> GetSettingsValues()
        {
            var settings = new List<EffectSetting>();
            settings.Add(new EffectSetting() { Code = 0, Value = ModeNumber }); // effect mode number
            settings.Add(new EffectSetting() { Code = 1, Value = _settingsModel.WestHue.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 2, Value = _settingsModel.EastHue.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 4, Value = _settingsModel.FanPhaseOffset.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 5, Value = _settingsModel.SidePhaseOffset.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 6, Value = (int)_settingsModel.Pulse.Key });
            settings.Add(new EffectSetting() { Code = 7, Value = _settingsModel.PulseSpeed.GetValueOrDefault() });
            return settings;
        }
    }
}
