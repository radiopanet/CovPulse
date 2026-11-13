using System.IO;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
// ==========================================
// CLI Settings (Flags & Arguments)
// ==========================================
public sealed class CoverageSettings : CommandSettings
{
    [CommandArgument(0, "[file]")]
    [Description("Path to the Cobertura or OpenCover XML file. If omitted, auto-discovers in TestResults/")]
    public string? FilePath { get; init; }

    [CommandOption("-j|--json")]
    [Description("Output raw JSON summary instead of rendering the Spectre.Console UI.")]
    public bool Json { get; init; }

    [CommandOption("-t|--threshold")]
    [DefaultValue(80.0)]
    [Description("Minimum coverage percentage threshold for green status.")]
    public double Threshold { get; init; }

    [CommandOption("-f|--file-filter")]
    [Description("Filter report to show specific file details.")]
    public string? FileFilter { get; init; }
}