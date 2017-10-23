using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets.Effects
{
    public class Bpm : IEffect
    {
        public string Name { get => "BPM"; }
        public int ModeNumber { get => 6; }
        public Guid Id { get => new Guid("F1D1F971-EF3C-49D1-B918-032DB6E278B3"); }
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
