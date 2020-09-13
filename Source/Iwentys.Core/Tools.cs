using System.Globalization;

namespace Iwentys.Core
{
    public static class Tools
    {
        public static bool ParseInAnyCulture(string value, out double result)
        {
            return double.TryParse(value.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }
    }
}