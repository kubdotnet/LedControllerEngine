using System.ComponentModel;
using System.Reflection;

namespace System
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the enum description attribute.
        /// System.ComponentModel.DescriptionAttribute must be specified (see example)
        /// </summary>
        /// <example>
        /// public enum MyEnum
        ///	{
        ///		[Description("Enum 1 description")] Enum1 = 0,
        ///		[Description("Enum 2 description")] Enum2 = 1,
        ///		[Description("Enum 3 description")] Enum3 = 2,
        ///		[Description("Enum 4 description")] Enum4 = 3
        /// }
        /// </example>
        /// <remarks>
        /// public enum MyEnum
        ///	{
        ///		[Description("Enum 1 description")] Enum1 = 0,
        ///		[Description("Enum 2 description")] Enum2 = 1,
        ///		[Description("Enum 3 description")] Enum3 = 2,
        ///		[Description("Enum 4 description")] Enum4 = 3
        /// }
        /// </remarks>
        public static string GetDescription(this Enum source)
        {
            FieldInfo _info = source.GetType().GetField(source.ToString());
            DescriptionAttribute[] _attr = (DescriptionAttribute[])_info.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (_attr.Length > 0) return _attr[0].Description;
            else return source.ToString();
        }
    }
}
