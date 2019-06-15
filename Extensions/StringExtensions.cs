using System;

namespace Archaic.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToUpperFirst(this string source)
        {
            if (String.IsNullOrEmpty(source))
                throw new ArgumentException("String is null or empty!");

            return source[0].ToString().ToUpper() + source.Substring(1);
        }

        public static string TrimEnd(this string source, string value)
        {
            if (!source.EndsWith(value))
                return source;

            return source.Remove(source.LastIndexOf(value));
        }

        public static string TrimStart(this string source, string value)
        {
            if (!source.StartsWith(value))
                return source;

            return source.Remove(source.IndexOf(value));
        }
    }
}