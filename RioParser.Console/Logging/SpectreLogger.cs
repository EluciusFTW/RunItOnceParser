using System;
using Spectre.Console;
using System.Collections.Generic;
using RioParser.Domain.Extensions;
using RioParser.Domain.Logging;
using Spectre.Console.Rendering;
using System.Linq;
using RioParser.Domain.Artefact;

namespace RioParser.Console.Logging
{
    internal class SpectreLogger : ILogger
    {
        public SpectreLogger()
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        public void Chapter(string line)
        {
            Lined("", "yellow");
            Lined(line, "yellow on blue");
            Lined("", "yellow");
            NewLine();
        }

        private void Lined(string line, string style, Justify justify = Justify.Left)
        {
            var rule = new Rule($"[{style}]{line}[/]")
            {
                Alignment = justify
            };
            AnsiConsole.Render(rule);
        }

        public void Log(string message) => Log(message, "cyan3");
        public void Log(string message, string styles) => AnsiConsole.MarkupLine($"[{styles}]{message}[/]");

        public void Paragraph(string line)
        {
            NewLine();
            Lined(line, "deepskyblue1");
        }

        private static void NewLine() => AnsiConsole.WriteLine(string.Empty);

        public void Verbose(string message) => Log(message);

        internal void LogArtefacts(IEnumerable<IReportArtefact> messages)
            => messages.ForEach(message => LogArtefact(message));

        internal void LogGroup(string title, IEnumerable<string> lines)
        {
            Chapter(title);
            lines.ForEach(line => Log(line, "yellow"));
        }

        private void LogArtefact(IReportArtefact artefact)
        {
            switch (artefact)
            {
                case TableArtefact table:
                    AnsiConsole.Render(ToTable(table));
                    break;
                case ValueCollectionArtefact list:
                    AnsiConsole.Render(ToBarChart(list));
                    break;
                case SimpleArtefact item:
                    LogSimple(item);
                    break;
                case GroupArtefact: return;
                case CollectionArtefact: return;
                default: throw new NotSupportedException();
            }
        }

        private void LogSimple(SimpleArtefact item)
        {
            switch (item.Level)
            {
                case ArtefactLevel.Heading:
                    {
                        NewLine();
                        Lined(item.Value, "deepskyblue1");
                        break;
                    }
                case ArtefactLevel.Info:
                    {
                        Log(item.Value);
                        break;
                    }
                case ArtefactLevel.Warning:
                    {
                        Log(item.Value, "darkorange3_1");
                        break;
                    }
                case ArtefactLevel.Error:
                    {
                        Log(item.Value, "red");
                        break;
                    }
                default: throw new NotImplementedException();
            }
        }

        private static IRenderable ToBarChart(ValueCollectionArtefact list)
        {
            var bar = new BarChart()
                .Width(80)
                .Label($"[cyan3 bold]{list.Title}[/]");
            list.Values.ForEach(line => bar.AddItem(line.Key, line.Value, Color.Aquamarine1));
            return bar;
        }

        private static IRenderable ToTable(TableArtefact table)
        {
            var output = new Table();

            table.Headers.ForEach(header => output.AddColumn(header));
            output.Columns[0].Width(25);
            output.Columns[1].Width(12);

            table.Rows.ForEach(row => output.AddRow(row.Select(cell => new Markup(cell, null))));
            return output;
        }
    }
}
