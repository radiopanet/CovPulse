
public class CliOptions
{
    public FileInfo InputFile {get; set;} = null!;
    public FileInfo? OutputFile {get; set;}
    public OutputFormat Format {get; set;} = OutputFormat.Text;
}

public enum OutputFormat
{
    Text,
    Json
}