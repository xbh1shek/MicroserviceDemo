
using System.Text.RegularExpressions;

namespace Catalogue.Domain.Entities
{
    public static class DomainValidator
    {
        public static string NotNullOrWhiteSpace(this string? input, string? parameterName = null)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new CatalogueException($"Required input {FormatParameter(parameterName)} was empty.", parameterName);
            return input;

        }
        public static string NotNullOrWhiteSpace(string? input, string? parameterName = null, string? message = null)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new CatalogueException(message ?? $"Required input {FormatParameter(parameterName)} was empty.", parameterName);
            return input;

        }
        public static int NotNegativeOrZero(this int input, string parameterName)
        {
            return NegativeOrZero<int>(input, parameterName, null);

        }

        public static long NotNegativeOrZero(this long input, string parameterName)
        {
            return NegativeOrZero<long>(input, parameterName, null);

        }

        public static int NotNegativeOrZero(this int input, string? parameterName = null, string? message = null)
        {
            return NegativeOrZero<int>(input, parameterName, message);

        }
        public static decimal NotNegativeOrZero(this decimal input, string? parameterName = null, string? message = null)
        {
            return NegativeOrZero<decimal>(input, parameterName, message);

        }

        public static T NotNull<T>(this T input, string? parameterName = null)
        {
            if (input is null)
                throw new CatalogueException($"Required input {FormatParameter(parameterName)} cannot be null.", parameterName);


            return input;
        }

        private static T Null<T>(T input, string? parameterName = null, string? message = null)
        {
            if (input is null)
                throw new CatalogueException(message ?? $"Required input {FormatParameter(parameterName)} cannot be null.", parameterName);


            return input;
        }

        private static T NegativeOrZero<T>(T input, string? parameterName = null, string? message = null) where T : struct, IComparable
        {
            if (input.CompareTo(default(T)) <= 0)
                throw new CatalogueException(message ?? $"Required input {FormatParameter(parameterName)} cannot be zero or negative.", parameterName);


            return input;
        }

        private static object FormatParameter(string? parameterName)
        {
            return char.ToUpper(parameterName[0]) + Regex.Replace(parameterName.Substring(1), @"\B[A-Z]", m => " " + m.Value);
        }
    }
}
