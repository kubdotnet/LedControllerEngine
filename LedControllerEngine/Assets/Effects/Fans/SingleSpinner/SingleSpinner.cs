﻿using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets.Effects
{
    public class SingleSpinner : IEffect
    {
        public string Name { get => "Single Spinner"; }
        public int ModeNumber { get => 1; }
        public Guid Id { get => new Guid("84534594-276F-49CE-82D9-87B4A79F0025"); }
        public Type SettingsControl
        {
            get => typeof(SingleSpinnerSettings);
        }

        private SingleSpinnerSettingsModel _settingsModel = new SingleSpinnerSettingsModel();
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
                    throw new ArgumentException("Wrong type, SingleSpinnerSettingsModel expected");
                }

                _settingsModel = (SingleSpinnerSettingsModel)value;
            }
        }

        public Type EffectType => typeof(SingleSpinner);

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
