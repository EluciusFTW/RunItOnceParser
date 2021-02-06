using Microsoft.Extensions.Configuration;
using RioParser.Domain;
using RioParser.Domain.Extensions;
using RioParser.Domain.Logging;
using RioParser.Domain.Reports;
using RioParser.Domain.Reports.Models;
using System.IO;

namespace RioParser.Console
{
    internal class ReportOptionsBuilder
    {
        private ILogger _logger;

        internal ReportOptionsBuilder(ILogger logger)
        {
            _logger = logger;
        }

        public (bool, ReportOptions) Build(string path, string hero, ReportType reportType, GameType gameType, bool verbose)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile($"configuration.json", true, true)
                .Build();

            var resolvedPath = !string.IsNullOrEmpty(path)
                ? path
                : config["Path"];

            var resolvedGameType = gameType != GameType.Unknown
                ? gameType
                : config["GameType"].GetEnumValue<GameType>();

            var resolvedReportType = reportType != ReportType.Unknown
               ? reportType
               : config["ReportType"].GetEnumValue<ReportType>();

            var resolvedHero = !string.IsNullOrEmpty(hero)
                ? hero
                : config["Hero"];

            if (string.IsNullOrEmpty(resolvedPath))
            {
                _logger.Paragraph($"A path is required, please provide it via \"--{nameof(path)} xxx\" or in the configuration.json.");
                return (false, null);
            }

            if (!Directory.Exists(resolvedPath))
            {
                _logger.Paragraph($"The path {resolvedPath} does not exist. Please set a valid path.");
                return (false, null);
            }

            if (verbose)
            {
                LogOptions(resolvedPath, resolvedHero, resolvedGameType, resolvedReportType);
            }
            
            return (true, new ReportOptions(resolvedPath, resolvedHero, resolvedGameType, resolvedReportType, verbose));
        }

        private  void LogOptions(string resolvedPath, string resolvedHero, GameType resolvedGameType, ReportType resolvedReportType)
        {
            _logger.Paragraph("Configuration");
            _logger.Log("- Report type:             " + resolvedReportType);
            _logger.Log("- Game type:               " + resolvedGameType);
            _logger.Log("- Hero name:               " + resolvedHero);
            _logger.Log("- Hand history files path: " + resolvedPath);
        }
    }
}
