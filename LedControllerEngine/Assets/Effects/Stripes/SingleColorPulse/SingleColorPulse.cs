using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets.Effects.Stripes
{
    public class SingleColorPulse : IEffect
    {
        public string Name { get => "Single Color Pulse"; }
        public int ModeNumber { get => 1; }
        public Guid Id { get => new Guid("AE07C086-C15A-44D0-A995-705847259F47"); }
        public Type SettingsControl
        {
            get => typeof(SingleColorPulseSettings);
        }

        private SingleColorPulseSettingsModel _settingsModel = new SingleColorPulseSettingsModel();
        public object SettingsModel
        {
            get
            {
                return _settingsModel;
            }
            set
            {
                if (!(value is SingleColorPulseSettingsModel))
                {
                    throw new ArgumentException("Wrong type, SingleColorPulseSettingsModel expected");
                }

                _settingsModel = (SingleColorPulseSettingsModel)value;
            }
        }

        public Type EffectType => typeof(Bpm);

        public DeviceType Compatiblity { get => DeviceType.Stripe; }

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
            settings.Add(new EffectSetting() { Code = 1, Value = _settingsModel.Hue.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 7, Value = _settingsModel.Speed.GetValueOrDefault() });
            return settings;
        }
    }
}
