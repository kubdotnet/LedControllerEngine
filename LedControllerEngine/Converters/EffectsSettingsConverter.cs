using LedControllerEngine.Assets;
using LedControllerEngine.Assets.Effects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LedControllerEngine.Converters
{
    public class EffectsSettingsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            List<IEffect> effects = new List<IEffect>();

            var jsonArray = JArray.Load(reader);
            foreach (var token in jsonArray)
            {
                var effectTypeToken = token.SelectToken("type");
                if (effectTypeToken != null)
                {
                    var effectType = Type.GetType(effectTypeToken.ToString());
                    if (effectType != null)
                    {
                        var effect = serializer.Deserialize(token.CreateReader(), effectType);
                        effects.Add(effect as IEffect);
                    }
                }
            }

            return effects;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        /// <summary>
        /// Gets the type of the effect.
        /// </summary>
        /// <param name="effectId">The effect identifier.</param>
        /// <returns></returns>
        private Type GetEffectType(Guid effectId)
        {
            if (effectId == new Guid("F1D1F971-EF3C-49D1-B918-032DB6E278B3"))
            {
                return typeof(Bpm);
            }

            return null;
        }
    }
}
