using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets.Effects.Stripes
{
    public class Bpm : IEffect
    {
        public string Name { get => "BPM"; }
        public int ModeNumber { get => 4; }
        public Guid Id { get => new Guid("D0BB10DD-B49B-44CB-8BC0-B7C6D3365591"); }
        public Type SettingsControl
        {
            get => typeof(BpmSettings);
        }

        private BpmSettingsModel _settingsModel = new BpmSettingsModel();
        public object SettingsModel
        {
            get
            {
                return _settingsModel;
            }
            set
            {
                if (!(value is BpmSettingsModel))
                {
                    throw new ArgumentException("Wrong type, BpmSettingsModel expected");
                }

                _settingsModel = (BpmSettingsModel)value;
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
            settings.Add(new EffectSetting() { Code = 2, Value = _settingsModel.Beat.GetValueOrDefault() });
            settings.Add(new EffectSetting() { Code = 7, Value = _settingsModel.Bpm.GetValueOrDefault() });
            return settings;
        }
    }
}
