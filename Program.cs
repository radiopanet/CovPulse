using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Spectre.Console;
using Spectre.Console.Cli;
using System.IO;
// using System.CommandLine;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var app = new CommandApp<CoverageCommand>();

        app.Configure(config =>
        {
            config.SetApplicationName("covpulse");
            config.SetApplicationVersion("1.0.0");

            // Friendly error messaging
            config.PropagateExceptions();
        });

        return await app.RunAsync(args);
    }
}