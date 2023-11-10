using System;
using System.Collections.Generic;
using System.Text;

namespace ChunkyConsole.Utils
{
    public static class NullableParseHelper
    {
        public delegate bool TP<T>(string a, out T ou);
        internal static Nullable<K> ParseNullable<K>(string a, TP<K> tp) where K : struct
        {
            K retval = default(K);
            return retval;
        }
        internal static Nullable<K> ParseNullable<K>(this K source, string a, TP<K> tp) where K : struct
        {
            K retv = default(K);
            if (tp(a, out retv))
                return retv;
            else
                return null;
        }
        public static decimal? ParseNullable(this decimal source, string a)
        {
            return source.ParseNullable(a, decimal.TryParse);
        }
        public static int? ParseNullable(this int source, string a)
        {
            return source.ParseNullable(a, int.TryParse);
        }
        public static long? ParseNullable(this long source, string a)
        {
            return source.ParseNullable(a, long.TryParse);
        }
        public static double? ParseNullable(this double source, string a)
        {
            return source.ParseNullable(a, double.TryParse);
        }
        public static DateTime? ParseNullable(this DateTime source, string a)
        {
            return source.ParseNullable(a, DateTime.TryParse);
        }
    }
}
