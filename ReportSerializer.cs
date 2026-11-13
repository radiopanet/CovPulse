using System.Text.Json;
using System.Text.Json.Serialization;

public static class ReportSerializer
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static string ToJson(CoverageReport report)
    {
        return JsonSerializer.Serialize(report, Options);
    }
}