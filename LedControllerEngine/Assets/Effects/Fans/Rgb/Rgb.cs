using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets.Effects
{
    public class Rgb : IEffect
    {
        public string Name { get => "RGB"; }
        public int ModeNumber { get => 9; }
        public Guid Id { get => new Guid("5BCDBFA8-EE44-4CF3-8DD2-247C20483207"); }
        public Type SettingsControl
        {
            get => typeof(RgbSettings);
        }

        private RgbSettingsModel _settingsModel = new RgbSettingsModel();
        public object SettingsModel
        {
            get
            {
                return _settingsModel;
            }
            set
            {
                if (!(value is RgbSettingsModel))
                {
                    throw new ArgumentException("Wrong type, RgbSettingsModel expected");
                }

                _settingsModel = (RgbSettingsModel)value;
            }
        }

        public Type EffectType => typeof(Rgb);

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
            settings.Add(new EffectSetting() { Code = 1, Value = _settingsModel.Color.R });
            settings.Add(new EffectSetting() { Code = 2, Value = _settingsModel.Color.G });
            settings.Add(new EffectSetting() { Code = 3, Value = _settingsModel.Color.B });
            return settings;
        }
    }
}
