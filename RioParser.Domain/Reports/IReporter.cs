using RioParser.Domain;
using RioParser.Domain.HandHistories;
using RioParser.Domain.Reports;
using System.Collections.Generic;

public interface IReporter
{
    public IReadOnlyCollection<IHandsReport> Process(IReadOnlyCollection<HandHistoryFile> handhistoryFiles, string hero, GameType gameType);
}