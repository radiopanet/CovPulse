using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

var app = new CommandApp<CoverageCommand>();

app.Configure(config =>
{
    config.SetApplicationName("covpulse");
    config.SetApplicationVersion("1.0.0");

    // Friendly error messaging
    config.PropagateExceptions();
});

return await app.RunAsync(args);

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

// ==========================================
// Main Coverage Command Execution
// ==========================================
public sealed class CoverageCommand : AsyncCommand<CoverageSettings>
{
    public override Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] CoverageSettings settings)
    {
        // 1. Resolve target file path (use explicit or auto-discover)
        var targetFile = settings.FilePath ?? AutoDiscoverCoverageFile();

        if (string.IsNullOrEmpty(targetFile) || !File.Exists(targetFile))
        {
            AnsiConsole.MarkupLine("[bold red]Error:[/] No valid coverage XML file found. Specify a path or run 'dotnet test --collect:\"XPlat Code Coverage\"'.");
            return Task.FromResult(1);
        }

        // 2. Render JSON mode or Terminal UI mode
        if (settings.Json)
        {
            RenderJsonSummary(targetFile);
        }
        else
        {
            RenderTerminalUi(targetFile, settings.Threshold);
        }

        return Task.FromResult(0);
    }
    private static string? AutoDiscoverCoverageFile()
    {
        var currentDir = Directory.GetCurrentDirectory();

        // Search standard dotnet test output paths
        return Directory.GetFiles(currentDir, "*.cobertura.xml", SearchOption.AllDirectories)
            .Concat(Directory.GetFiles(currentDir, "coverage.xml", SearchOption.AllDirectories))
            .FirstOrDefault();
    }

    private static void RenderJsonSummary(string filePath)
    {
        // Placeholder for JSON serialization step
        AnsiConsole.WriteLine($$"""{"status": "success", "file": "{{filePath}}", "summary": "JSON output mode"}""");
    }

    private static void RenderTerminalUi(string filePath, double threshold)
    {
        AnsiConsole.MarkupLine($"[bold blue]CovPulse[/] parsing [yellow]{Path.GetFileName(filePath)}[/]...");

        // Placeholder for Spectre.Console Table / Panel rendering
        var table = new Table();
        table.AddColumn("File");
        table.AddColumn("Coverage");
        table.AddRow("Services/OrderService.cs", $"[green]92.5%[/]");
        table.AddRow("Controllers/PaymentController.cs", $"[red]45.0%[/]");

        AnsiConsole.Write(table);
    }
}