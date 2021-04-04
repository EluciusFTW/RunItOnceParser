﻿using System;
using System.Collections.Generic;

namespace RioParser.Domain.Artefact
{
    public class CollectionArtefact : IReportArtefact
    {
        public CollectionArtefact(string title, IReadOnlyCollection<string> items)
        {
            Title = title;
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public string Title { get; }
        public IReadOnlyCollection<string> Items { get; }
    }
}
