using System.Xml;
using System.Xml.Linq;

public static class CoberturaParser
{
    public static CoverageReport Parse(string filePath)
    {
        var doc = XDocument.Load(filePath);
        var root = doc.Root;

        if(root == null || root.Name.LocalName != "coverage")
        {
            throw new InvalidOperationException("Invalid or unsupported Cobertura XML format.");
        }

        // 1. Extract global line-rate (Cobertura stores it as a decimal, e.g., 0.925 -> 92.5%)
        double totalLineRate = double.TryParse(root.Attribute("line-rate")?.Value, out var rate)
            ? rate * 100
            : 0.0;

        // 2. Query all <class> elements (each represents a source file)
        var files = root.Descendants("class")
            .Select(cls => new FileCoverage(
                FileName: cls.Attribute("filename")?.Value ?? "Unknown",
                LineRate: double.TryParse(cls.Attribute("line-rate")?.Value, out var fileRate) ? fileRate * 100 : 0.0,
                Lines: cls.Descendants("line")
                    .Select(l =>
                    {
                        int hits = int.TryParse(l.Attribute("hits")?.Value, out var h) ? h : 0;
                        int number = int.TryParse(l.Attribute("number")?.Value, out var num) ? num : 0;

                        return new LineInfo(
                            Number: number,
                            Hits: hits,
                            Covered: hits > 0
                        );
                    })
                    .ToList()
            ))
            .Where(f => f.FileName != "Unknown")
            .ToList();

        return new CoverageReport(
            ProjectName: Path.GetFileNameWithoutExtension(filePath),
            TotalLineRate: totalLineRate,
            Files: files
        );
    }
}