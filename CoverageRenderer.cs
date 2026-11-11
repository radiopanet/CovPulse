using Spectre.Console;

public static class CoverageRenderer
{
    public static void RenderReport(CoverageReport report, double threshold)
    {
        //1. Determine overall status color based on threshold
        var overallColor = report.TotalLineRate >= threshold ? "green" : "red";

        //2. Build a summary panel header
        var headerMarkup = new Markup(
            $"[bold]Project:[/] [cyan]{report.ProjectName}[/]\n" +
            $"[bold]Total Line Coverage:[/] [{overallColor}]{report.TotalLineRate:F2}%[/] " +
            $"(Target Threshold: [yellow]{threshold}%[/])"
        );

        AnsiConsole.Write(new Panel(headerMarkup)
        {
            Header = new PanelHeader(" CovPulse Summary ", Justify.Center),
            Border = BoxBorder.Rounded,
            Padding = new Padding(2, 1, 2, 1)
        });

        //3. Build the File Breakdown Table
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn(new TableColumn("[bold]Source File[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold]Line Rate[/]").RightAligned());
        table.AddColumn(new TableColumn("[bold]Status[/]").Centered());

        foreach (var file in report.Files.OrderBy(f => f.LineRate))
        {
            //Color code individual files based on threshold proximity
            var fileColor = file.LineRate >= threshold ? "green" : (file.LineRate >= (threshold - 20) ? "yellow" : "red");
            var statusBadge = file.LineRate >= threshold ? "[green]PASS[/]" : "[red]LOW[/]";

            //Clean up file path representation for display
            var displayPath = Markup.Escape(file.FileName);

            table.AddRow(
                displayPath,
                $"[{fileColor}]{file.LineRate:F2}%[/]",
                statusBadge
            );
        }
        AnsiConsole.Write(table);
    }
}