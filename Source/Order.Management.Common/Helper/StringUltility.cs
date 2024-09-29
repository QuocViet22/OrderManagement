
using System.Globalization;

namespace OrderManagement.Common.Helper
{
    public static class StringUltility
    {
        /// <summary>
        /// Convert string to datetime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ConvertStringToDateTime(string? value)
        {
            DateTime result;
            if(!string.IsNullOrWhiteSpace(value) && DateTime.TryParse(value, CultureInfo.InvariantCulture, out result))
                return result;
            return null;
        }
    }
}
