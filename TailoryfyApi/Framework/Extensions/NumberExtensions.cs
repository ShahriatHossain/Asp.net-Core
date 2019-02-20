using Microsoft.Extensions.Primitives;
using System;

namespace Framework.Extensions
{
    public static class NumberExtensions
    {
        public static int? ToNullableInt(this StringValues value)
        {
            return ToNullableInt(Convert.ToString(value));
        }
        public static int? ToNullableInt(this string value)
        {
            int v;
            if (int.TryParse(value, out v))
            {
                return v;
            }
            return null;
        }

        public static bool? ToNullableBoolean(this StringValues value)
        {
            return ToNullableBoolean(Convert.ToString(value));
        }
        public static bool? ToNullableBoolean(this string value)
        {
            bool v;
            if (bool.TryParse(value, out v))
            {
                return v;
            }
            return null;
        }
    }
}
