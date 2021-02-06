using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace RioParser.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string BetweenSingle(this string subject, string beginning, string end)
            => subject.AfterFirst(beginning).Before(end);

        public static string Before(this string subject, string end)
            => subject.Split(end)[0].Trim();

        public static string BeforeAny(this string subject, string[] ends)
        {
            var piece = subject;
            ends.ForEach(end => piece = piece.Split(end)[0]);

            return piece.Trim();
        }

        public static string AfterSingle(this string subject, string marker)
        {
            return AfterSingleOrDefault(subject, marker)
                ?? throw new ArgumentException($"The marker string \"{marker}\" is contained not contained in \"{subject}\", but expected.");
        }

        public static string AfterSingleOrDefault(this string subject, string marker)
        {
            var parts = subject.Split(marker);
            return parts.Length == 2
                ? subject.Split(marker)[1].Trim()
                : parts.Length == 1 
                    ? null
                    : throw new ArgumentException($"The marker string \"{marker}\" is contained {parts.Length - 1} times in \"{subject}\", but is expected to be contained at most once.");
        }


        public static string AfterFirst(this string subject, string marker)
        {
            var startIndex = subject.IndexOf(marker);
            return startIndex >= 0
                ? subject.Substring(startIndex + marker.Length).Trim()
                : throw new ArgumentException($"The marker string \"{marker}\" is not contained in \"{subject}\"");
        }

        public static string LineContaining(this string subject, string marker)
            => subject
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(line => line.Contains(marker)) ?? string.Empty;

        public static string[] SplitIntoLines(this string subject)
            => Regex.Split(subject, "\r\n|\r|\n");

        public static decimal ToDecimal(this string subject)
            => !string.IsNullOrEmpty(subject)
                ? Convert.ToDecimal(subject, CultureInfo.InvariantCulture)
                : 0;

        public static string TakeAfter(this string subject, char marker, int digits)
        {
            var markerIndex = subject.IndexOf(marker);
            return markerIndex >= 0
                ? subject.Substring(0, markerIndex + digits + 1)
                : string.Empty;
        }

        public static T GetEnumValue<T>(this string str) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }

            T val = ((T[])Enum.GetValues(typeof(T)))[0];
            if (!string.IsNullOrEmpty(str))
            {
                foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
                {
                    if (enumValue.ToString().ToUpper().Equals(str.ToUpper()))
                    {
                        val = enumValue;
                        break;
                    }
                }
            }

            return val;
        }
    }
}
