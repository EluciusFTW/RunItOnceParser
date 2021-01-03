using System;
using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
            => source
                .Indexed()
                .ForEach(element => action(element.Item, element.Index));

        private static IEnumerable<(T Item, int Index)> Indexed<T>(this IEnumerable<T> source) 
            => source
                .Select((item, index) => (item, index));
    }
}
