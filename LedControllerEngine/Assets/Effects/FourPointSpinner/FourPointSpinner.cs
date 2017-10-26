using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets.Effects
{
    public class FourPointSpinner : IEffect
    {
        public string Name { get => "Four Point Spinner"; }
        public int ModeNumber { get => 3; }
        public Guid Id { get => new Guid("73A5A86B-B773-401B-A0ED-58D63C1A5B59"); }
        public Type SettingsControl
        {
            get => typeof(SingleSpinnerSettings);
        }

        private FourPointSpinnerSettingsModel _settingsModel = new FourPointSpinnerSettingsModel();
        public object SettingsModel
        {
            get
            {
                return _settingsModel;
            }
            set
            {
                if (!(value is FourPointSpinnerSettingsModel))
                {
                    throw new ArgumentException("Wrong type, FourPointSpinnerSettingsModel expected");
                }

                _settingsModel = (FourPointSpinnerSettingsModel)value;
            }
        }

        public Type EffectType => typeof(FourPointSpinner);

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
            settings.Add(new EffectSetting() { Code = 1, Value = _settingsModel.Hue.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 2, Value = (int)_settingsModel.Direction });
            settings.Add(new EffectSetting() { Code = 3, Value = (int)_settingsModel.Mode });
            settings.Add(new EffectSetting() { Code = 4, Value = _settingsModel.ChangeRate.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 5, Value = _settingsModel.BladeOffset.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 6, Value = _settingsModel.FadeSpeed.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 7, Value = _settingsModel.Speed.GetValueOrDefault() });
            return settings;
        }
    }
}
