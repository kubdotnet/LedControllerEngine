using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace LedControllerEngine.Assets
{
    public class Settings : ObservableObject
    {
        private string _port;
        [JsonProperty("port")]
        public string Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
                RaisePropertyChanged(() => Port);
            }
        }

        [JsonIgnore]
        public IEnumerable<string> PortList { get; set; }

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
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(file));
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
