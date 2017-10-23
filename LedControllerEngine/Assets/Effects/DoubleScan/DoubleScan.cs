using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets.Effects
{
    public class DoubleScan: IEffect
    {
        public string Name { get => "Double Scan"; }
        public int ModeNumber { get => 4; }
        public Guid Id { get => new Guid("71562072-8981-4276-9C72-DCE37B8D274A"); }
        public Type SettingsControl
        {
            get => typeof(DoubleScanSettings);
        }

        private DoubleScanSettingsModel _settingsModel = new DoubleScanSettingsModel();
        public object SettingsModel
        {
            get
            {
                return _settingsModel;
            }
            set
            {
                if (!(value is DoubleScanSettingsModel))
                {
                    throw new ArgumentException("Wrong type, DoubleScanSettingsModel expected");
                }

                _settingsModel = (DoubleScanSettingsModel)value;
            }
        }

        public Type EffectType => typeof(DoubleScan);

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
            settings.Add(new EffectSetting() { Code = 2, Value =_settingsModel.Rotation.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 3, Value = (int)_settingsModel.Mode });
            settings.Add(new EffectSetting() { Code = 4, Value = _settingsModel.ChangeRate.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 5, Value = _settingsModel.BladeOffset.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 6, Value = _settingsModel.FadeSpeed.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 7, Value = _settingsModel.Speed.GetValueOrDefault() });
            return settings;
        }
    }
}
