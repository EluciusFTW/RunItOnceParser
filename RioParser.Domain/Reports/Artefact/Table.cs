using System;
using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Reports.Artefact
{
    public class Table : IReportArtefact
    {
        public Table(IReadOnlyCollection<string> headers, IEnumerable<IReadOnlyCollection<string>> rows)
        {
            Headers = headers;
            Rows = rows;
        }

        public IReadOnlyCollection<string> Headers { get; }
        public IEnumerable<IReadOnlyCollection<string>> Rows { get; }
    }

    public class List : IReportArtefact
    {
    }

    public class Group : IReportArtefact
    {
    }

    public class ValueList : IReportArtefact
    {
        public string Title { get; }
        public IDictionary<string, int> Values { get; }

        public ValueList(string title, IEnumerable<KeyValuePair<string, int>> values, bool orderByValues = false)
        {
            var orderedValues = orderByValues
                ? values.OrderBy(kvp => kvp.Value)
                : values;

            Values = orderedValues.ToDictionary(x => x.Key, x => x.Value);
            Title = title;
        }
    }

    public class Item : IReportArtefact
    {
        public string Value { get; }

        public Item(string value)
        {
            Value = value ?? throw new ArgumentNullException();
        }
    }
}
