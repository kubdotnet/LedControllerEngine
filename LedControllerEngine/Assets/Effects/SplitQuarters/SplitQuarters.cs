using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets.Effects
{
    public class SplitQuarters: IEffect
    {
        public string Name { get => "Split Quarters"; }
        public int ModeNumber { get => 8; }
        public Guid Id { get => new Guid("8995DECC-44AD-45E7-A8D1-F6F3FE5BDEB8"); }
        public Type SettingsControl
        {
            get => typeof(SplitQuartersSettings);
        }

        private SplitQuartersSettingsModel _settingsModel = new SplitQuartersSettingsModel();
        public object SettingsModel
        {
            get
            {
                return _settingsModel;
            }
            set
            {
                if (!(value is SplitQuartersSettingsModel))
                {
                    throw new ArgumentException("Wrong type, SplitQuartersSettingsModel expected");
                }

                _settingsModel = (SplitQuartersSettingsModel)value;
            }
        }

        public Type EffectType => typeof(SplitQuarters);

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
            settings.Add(new EffectSetting() { Code = 1, Value = _settingsModel.NorthWestHue.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 2, Value = _settingsModel.NorthEastHue.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 3, Value = _settingsModel.SouthEastHue.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 4, Value = _settingsModel.SouthWestHue.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 5, Value = _settingsModel.PulsePhaseOffset.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 6, Value = (int)_settingsModel.Pulse.Key });
            settings.Add(new EffectSetting() { Code = 7, Value = _settingsModel.PulseSpeed.GetValueOrDefault() });
            return settings;
        }
    }
}
