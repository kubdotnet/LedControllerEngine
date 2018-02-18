using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace LedControllerEngine.Assets
{
    public class Settings : ObservableObject
    {
        private string _port;
        [JsonProperty("port")]
        private string Port
        {
            set
            {
                _port = value;
            }
        }

        private List<string> _ports;
        [JsonProperty("ports")]
        public List<string> Ports
        {
            get
            {
                return _ports;
            }
            set
            {
                _ports = value;
                RaisePropertyChanged(() => Ports);
            }
        }

        private int _rate;
        [JsonProperty("rate")]
        public int Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                _rate = value;
               RaisePropertyChanged(() => Rate);
            }
        }

        [JsonIgnore]
        public IEnumerable<int> RateList { get; set; }

        private int _fans;
        [JsonProperty("fans")]
        
        public int FansCount
        {
            get
            {
                return _fans;
            }
            set
            {
                _fans = value;
                RaisePropertyChanged(() => FansCount);
            }
        }

        private int _stripes;
        [JsonProperty("stripes")]

        public int StripesCount
        {
            get
            {
                return _stripes;
            }
            set
            {
                _stripes = value;
                RaisePropertyChanged(() => StripesCount);
            }
        }

        private List<IEffect> _effects = new List<IEffect>();
        [JsonProperty("effects")]
        [JsonConverter(typeof(Converters.EffectsSettingsConverter))]
        public List<IEffect> Effects
        {
            get
            {
                return _effects;
            }
            set
            {
                _effects = value;
                RaisePropertyChanged(() => Effects);
            }
        }

        /// <summary>
        /// Loads from file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static Settings LoadFromFile(string file)
        {
            var settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(file));

            if (!string.IsNullOrEmpty(settings._port) && (settings.Ports == null || settings.Ports.Count == 0))
            {
                settings.Ports = new List<string>() { settings._port };
            }

            return settings;
        }

        /// <summary>
        /// Saves the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        public void Save(string file)
        {
            File.WriteAllText(file, JsonConvert.SerializeObject(this, new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                }
            ));
        }
    }
}
