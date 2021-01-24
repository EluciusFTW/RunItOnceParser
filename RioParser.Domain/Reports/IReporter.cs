using System.Collections.Generic;
using RioParser.Domain.HandHistories;

namespace RioParser.Domain.Reports
{
    public interface IReporter
    {
        public IReadOnlyCollection<IReport> Process(IReadOnlyCollection<HandHistoryFile> handHistoryFiles);
    }
}