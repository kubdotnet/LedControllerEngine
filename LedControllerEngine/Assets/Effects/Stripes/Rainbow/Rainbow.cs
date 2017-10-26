using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets.Effects.Stripes
{
    public class Rainbow : IEffect
    {
        public string Name { get => "Rainbow"; }
        public int ModeNumber { get => 3; }
        public Guid Id { get => new Guid("F464BE74-AB9F-475F-9139-3E10C5A1C738"); }
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
            settings.Add(new EffectSetting() { Code = 7, Value = _settingsModel.Speed.GetValueOrDefault() });
            return settings;
        }
    }
}
