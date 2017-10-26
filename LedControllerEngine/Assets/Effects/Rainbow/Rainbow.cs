using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets.Effects
{
    public class Rainbow : IEffect
    {
        public string Name { get => "Rainbow"; }
        public int ModeNumber { get => 2; }
        public Guid Id { get => new Guid("D0CF4901-40BD-4947-9C92-59F59D732A32"); }
        public Type SettingsControl
        {
            get => typeof(RainbowSettings);
        }

        private RainbowSettingsModel _settingsModel = new RainbowSettingsModel();
        public object SettingsModel
        {
            get
            {
                return _settingsModel;
            }
            set
            {
                if (!(value is RainbowSettingsModel))
                {
                    throw new ArgumentException("Wrong type, RainbowSettingsModel expected");
                }

                _settingsModel = (RainbowSettingsModel)value;
            }
        }

        public Type EffectType => typeof(Rainbow);

        public DeviceType Compatiblity { get => DeviceType.Fan; }

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
            settings.Add(new EffectSetting() { Code = 1, Value = _settingsModel.Sparkles.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 2, Value = _settingsModel.HueSteps.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 7, Value = _settingsModel.Speed.GetValueOrDefault() });
            return settings;
        }
    }
}
