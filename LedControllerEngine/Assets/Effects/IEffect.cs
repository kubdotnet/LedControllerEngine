using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LedControllerEngine.Assets
{
    public interface IEffect
    {
        /// <summary>
        /// Gets the type of the effect.
        /// </summary>
        /// <value>
        /// The type of the effect.
        /// </value>
        [JsonProperty("type")]
        Type EffectType { get; }

        /// <summary>
        /// Gets the name
        /// </summary>
        /// <value>
        /// Name of the effect
        /// </value>
        [JsonIgnore]
        string Name { get; }

        /// <summary>
        /// Gets the mode n umber.
        /// </summary>
        /// <value>
        /// The mode n umber.
        /// </value>
        [JsonIgnore]
        int ModeNumber { get; }

        /// <summary>
        /// Gets the unique identifier used to save and reload settings
        /// </summary>
        /// <value>
        /// Unique identifier.
        /// </value>
        [JsonIgnore]
        Guid Id { get; }

        /// <summary>
        /// Gets or sets the settings control.
        /// </summary>
        /// <value>
        /// The type of the settings control.
        /// </value>
        [JsonIgnore]
        Type SettingsControl { get; }

        /// <summary>
        /// Gets or sets the settings model.
        /// </summary>
        /// <value>
        /// The settings model.
        /// </value>
        [JsonProperty("settings")]
        object SettingsModel { get; set; }

        /// <summary>
        /// Gets the settings values.
        /// </summary>
        /// <returns>List of EffectSetting</returns>
        List<EffectSetting> GetSettingsValues();
    }
}
