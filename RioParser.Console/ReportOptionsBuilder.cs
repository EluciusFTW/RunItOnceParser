using Microsoft.Extensions.Configuration;
using RioParser.Console.Logging;
using RioParser.Domain;
using RioParser.Domain.Artefact;
using RioParser.Domain.Extensions;
using RioParser.Domain.Reports;
using System.IO;

namespace RioParser.Console
{
    internal class ReportOptionsBuilder
    {
        private readonly SpectreLogger _logger;

        internal ReportOptionsBuilder(SpectreLogger logger)
        {
            _logger = logger;
        }

        public (bool, ReportOptions) Build(string path, string hero, ReportType reportType, GameType gameType, bool verbose)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("configuration.json", true, true)
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
                _logger.LogArtefacts(
                    new[] { new SimpleArtefact($"A path is required, please provide it via \"--{nameof(path)} xxx\" or in the configuration.json.", ArtefactLevel.Error) });
                return (false, null);
            }

            if (!Directory.Exists(resolvedPath))
            {
                _logger.LogArtefact(
                    new SimpleArtefact($"The path {resolvedPath} does not exist. Please set a valid path.", ArtefactLevel.Error));
                return (false, null);
            }

            if (verbose)
            {
                LogOptions(resolvedPath, resolvedHero, resolvedGameType, resolvedReportType);
            }

            return (true, new ReportOptions(resolvedPath, resolvedHero, resolvedGameType, resolvedReportType, verbose));
        }

        private void LogOptions(string resolvedPath, string resolvedHero, GameType resolvedGameType, ReportType resolvedReportType)
        {
            var configuration = new[]
            {
                "- Report type:             " + resolvedReportType,
                "- Game type:               " + resolvedGameType,
                "- Hero name:               " + resolvedHero,
                "- Hand history files path: " + resolvedPath
            };

            var group = new CollectionArtefact("Configuration", configuration);
            _logger.LogArtefact(group);
        }
    }
}
