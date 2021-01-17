using System;
using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.HandHistories;
using RioParser.Domain.Logging;
using RioParser.Domain.Reports.Models;

namespace RioParser.Domain.Reports
{
    internal class Reporter<TReport> : IReporter where TReport: IHandsReport
    {
        private readonly ILogger _logger;
        private readonly ReportOptions _reportOptions;
    
        public Reporter(ReportOptions reportOptions, ILogger logger)
        {
            _reportOptions = reportOptions;
            _logger = logger;
        }

        public IReadOnlyCollection<IHandsReport> Process(IReadOnlyCollection<HandHistoryFile> handHistoryFiles)
        {
            var filteredFiles = handHistoryFiles
                .Where(file => file.Hands.First().Game == _reportOptions.GameType)
                .SelectMany(file =>
                {
                    _logger.Verbose($"Processing {file.Name} containing {file.Hands.Count} hands.");
                    return file.Hands;
                })
                .ToList();

            return new[] { (TReport)Activator.CreateInstance(typeof(TReport), _reportOptions, filteredFiles) }
                .Cast<IHandsReport>()
                .ToList();
        }
            
    }
}