using System;
using System.Windows.Data;

namespace LedControllerEngine.Converters
{
    /// <summary>
    /// Enumeration to boolean converter helper
    /// </summary>
    public class EnumToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Compare the enumeration with the specified object
        /// </summary>
        /// <param name="value">Target</param>
        /// <param name="targetType">Type of the enumeration</param>
        /// <param name="parameter">Object to compare to</param>
        /// <param name="culture">Culture used for compare</param>
        /// <returns>True or False</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null) return null;

            if (parameter.ToString().Equals(value.ToString())) return true;
            else return false;
        }

        /// <summary>
        /// Convert back the object to its enumeration
        /// </summary>
        /// <param name="value">Target</param>
        /// <param name="targetType">Type of the enumeration</param>
        /// <param name="parameter">Enumeration</param>
        /// <param name="culture">Culture used for compare</param>
        /// <returns>The original enumeration</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null) return null;

            bool _value = (bool)value;
            string targetValue = parameter.ToString();
            if (_value) return Enum.Parse(targetType, targetValue);

            return null;
        }
    }
}
