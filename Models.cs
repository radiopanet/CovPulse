public record CoverageReport(
    string ProjectName,
    double TotalLineRate,
    IReadOnlyList<FileCoverage> Files
);

public record FileCoverage(
    string FileName,
    double LineRate,
    IReadOnlyList<LineInfo> Lines
);

public record LineInfo(
    int Number,
    int Hits,
    bool Covered
);

