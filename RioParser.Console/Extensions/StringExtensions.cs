﻿using System;
using System.Globalization;
using System.Linq;

namespace RioParser.Console.Extensions
{
    public static class StringExtensions
    {
        public static string BetweenSingle(this string subject, string beginning, string end)
            => subject.AfterSingle(beginning).Before(end);

        public static string Before(this string suject, string end)
        {
            var parts = suject.Split(end);
            return end.Length >= 2
                ? parts[0].Trim()
                : string.Empty;
        }

        public static string AfterSingle(this string subject, string beginning)
        {
            var parts = subject.Split(beginning);
            return parts.Length == 2
                ? subject.Split(beginning)[1].Trim()
                : string.Empty;
        }

        public static string AfterFirst(this string subject, string beginning)
        {
            var parts = subject.Split(beginning);
            return parts.Length >= 2
                ? subject.Split(beginning)[1].Trim()
                : string.Empty;
        }

        public static string LineContaining(this string subject, string marker)
            => subject
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(line => line.Contains(marker)) ?? string.Empty;

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
    }
}
