using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets.Effects.Stripes
{
    public class SingleColor : IEffect
    {
        public string Name { get => "Single Color"; }
        public int ModeNumber { get => 1; }
        public Guid Id { get => new Guid("8630C50E-3DDA-4143-989E-5C4B98FA4CCB"); }
        public Type SettingsControl
        {
            get => typeof(SingleColorSettings);
        }

        private SingleColorSettingsModel _settingsModel = new SingleColorSettingsModel();
        public object SettingsModel
        {
            get
            {
                return _settingsModel;
            }
            set
            {
                if (!(value is SingleColorSettingsModel))
                {
                    throw new ArgumentException("Wrong type, SingleColorSettingsModel expected");
                }

                _settingsModel = (SingleColorSettingsModel)value;
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
            return settings;
        }
    }
}
