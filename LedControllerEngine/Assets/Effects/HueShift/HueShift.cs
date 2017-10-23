using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets.Effects
{
    public class HueShift : IEffect
    {
        public string Name { get => "HUE Shift"; }
        public int ModeNumber { get => 0; }
        public Guid Id { get => new Guid("41E0E2BC-A27E-472E-9856-546315F2E383"); }
        public Type SettingsControl
        {
            get => typeof(HueShiftSettings);
        }

        private HueShiftSettingsModel _settingsModel = new HueShiftSettingsModel();
        public object SettingsModel
        {
            get
            {
                return _settingsModel;
            }
            set
            {
                if (!(value is HueShiftSettingsModel))
                {
                    throw new ArgumentException("Wrong type, HueShiftSettingsModel expected");
                }

                _settingsModel = (HueShiftSettingsModel)value;
            }
        }

        public Type EffectType => typeof(HueShift);

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
            settings.Add(new EffectSetting() { Code = 1, Value = _settingsModel.StartingHue.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 2, Value = _settingsModel.EndingHue.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 3, Value = _settingsModel.HueOffset.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 5, Value = _settingsModel.PhaseOffset.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 7, Value = _settingsModel.Speed.GetValueOrDefault() });
            return settings;
        }
    }
}
