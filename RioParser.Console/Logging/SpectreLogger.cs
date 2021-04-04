using System;
using Spectre.Console;
using System.Collections.Generic;
using RioParser.Domain.Extensions;
using Spectre.Console.Rendering;
using System.Linq;
using RioParser.Domain.Artefact;

namespace RioParser.Console.Logging
{
    internal class SpectreLogger
    {
        private const string MessageStyle = "cyan3";
        private const string WarningStyle = "darkorange3_1";
        private const string ErrorStyle = "red";
        private const string ParagraphColor = "deepskyblue1";

        private Color[] BarChartColors = new[]
        {
            new Color(215,155,215),
            new Color(215,155,195),
            new Color(215,155,175),
            new Color(215,155,155),
            new Color(215,155,135)
        };

        private bool _verbosity;
        internal void SetVerbosity(bool verbosity) => _verbosity = verbosity;

        public SpectreLogger()
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        private void Lined(string line, string style, Justify justify = Justify.Left)
        {
            var rule = new Rule($"[{style}]{line}[/]")
            {
                Alignment = justify
            };
            AnsiConsole.Render(rule);
        }

        public void Log(string message) => Log(message, MessageStyle);
        public void Log(string message, string styles) => AnsiConsole.MarkupLine($"[{styles}]{message}[/]");

        public void Paragraph(string line)
        {
            NewLine();
            Lined(line, ParagraphColor);
        }

        private static void NewLine() => AnsiConsole.WriteLine(string.Empty);

        internal void LogArtefacts(IEnumerable<IReportArtefact> messages)
            => messages.ForEach(message => LogArtefact(message));

        internal void LogTitle(string title, IEnumerable<string> lines)
        {
            AnsiConsole.Render(new Rule());
            AnsiConsole.Render(new Rule());
            AnsiConsole.Render(new Rule());
            AnsiConsole.Render(
                new FigletText("RioParser")
                    .LeftAligned()
                    .Color(Color.Yellow));
            AnsiConsole.Render(new Rule());
            AnsiConsole.Render(new Rule());
            lines.ForEach(line => Log(line, "yellow"));
            AnsiConsole.Render(new Rule());
            AnsiConsole.Render(new Rule());
        }

        internal void LogArtefact(IReportArtefact artefact)
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
                case GroupArtefact: break;
                case CollectionArtefact item:
                    Lined(item.Title, ParagraphColor);
                    item.Items.ForEach(item => Log(item));
                    break;
                default: throw new NotSupportedException();
            }
        }

        private void LogSimple(SimpleArtefact item)
        {
            switch (item.Level)
            {
                case ArtefactLevel.Heading:
                    {
                        Paragraph(item.Value);
                        break;
                    }
                case ArtefactLevel.Info:
                    {
                        Log(item.Value);
                        break;
                    }
                case ArtefactLevel.Warning:
                    {
                        Log(item.Value, WarningStyle);
                        break;
                    }
                case ArtefactLevel.Error:
                    {
                        Log(item.Value, ErrorStyle);
                        break;
                    }
                default: throw new NotImplementedException();
            }
        }

        private IRenderable ToBarChart(ValueCollectionArtefact list)
        {
            var bar = new BarChart()
                .Width(80)
                .Label($"[{MessageStyle}]{list.Title}[/]");
            list.Values
                .ForEach((line, index) => bar.AddItem(line.Key, line.Value, BarChartColors[index % BarChartColors.Length]));
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
