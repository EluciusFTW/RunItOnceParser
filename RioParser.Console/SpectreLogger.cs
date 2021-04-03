using System;
using Spectre.Console;
using RioParser.Domain.Reports.Artefact;
using System.Collections.Generic;
using RioParser.Domain.Extensions;
using RioParser.Domain.Logging;
using Spectre.Console.Rendering;
using System.Linq;

namespace RioParser.Console
{
    internal class SpectreLogger : ILogger
    {
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
                case Domain.Reports.Artefact.Table table:
                    AnsiConsole.Render(ToTable(table));
                    break;
                case ValueList list:
                    AnsiConsole.Render(ToBarChart(list));
                    break;
                case Item item:
                    NewLine();
                    Lined(item.Value, "deepskyblue1");
                    break;
                case Group: return;
                case List: return;
                default: throw new NotSupportedException();
            }
        }

        private static IRenderable ToBarChart(ValueList list)
        {
            var bar = new BarChart()
                .Width(80)
                .Label($"[cyan3 bold]{list.Title}[/]");
            list.Values.ForEach(line => bar.AddItem(line.Key, line.Value, Color.Aquamarine1));
            return bar;
        }

        private static IRenderable ToTable(Domain.Reports.Artefact.Table table)
        {
            var output = new Spectre.Console.Table();

            table.Headers.ForEach(header => output.AddColumn(header));
            output.Columns[0].Width(25);
            output.Columns[1].Width(12);

            table.Rows.ForEach(row => output.AddRow(row.Select(cell => new Markup(cell, null))));
            return output;
        }
    }
}
