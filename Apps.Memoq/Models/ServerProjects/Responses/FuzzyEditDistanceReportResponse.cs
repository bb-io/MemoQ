using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Models.ServerProjects.Responses
{
    public class FuzzyEditDistanceReportResponse
    {
        [Display("Report ID")] public string? ReportId { get; set; }
        [Display("Total")] public FuzzyCountsDto? Total { get; set; }
        [Display("By language")] public List<FuzzyLanguageDto>? ByLanguage { get; set; }
    }

    public class FuzzyLanguageDto
    {
        [Display("Language code")] public string? LanguageCode { get; set; }
        [Display("Documents")] public List<FuzzyDocumentDto>? Documents { get; set; }
    }

    public class FuzzyDocumentDto
    {
        [Display("Document ID")] public string? DocumentId { get; set; }
        [Display("Document name")] public string? DocumentName { get; set; }
        [Display("Average fuzzy distance")] public double? AverageFuzzyDistance { get; set; }
        [Display("Average match rate")] public double? AverageMatchRate { get; set; }

        [Display("Total")] public FuzzyCountsDto? Total { get; set; }
        [Display("No match inserted")] public FuzzyCountsDto? NoMatchInserted { get; set; }
        [Display("Machine translated")] public FuzzyCountsDto? MachineTranslated { get; set; }
        [Display("X-translated")] public FuzzyCountsDto? XTranslated { get; set; }
        [Display("50–74%")] public FuzzyCountsDto? Rate50_74 { get; set; }
        [Display("75–84%")] public FuzzyCountsDto? Rate75_84 { get; set; }
        [Display("85–94%")] public FuzzyCountsDto? Rate85_94 { get; set; }
        [Display("95–99%")] public FuzzyCountsDto? Rate95_99 { get; set; }
        [Display("100%")] public FuzzyCountsDto? Rate100 { get; set; }
        [Display("101%")] public FuzzyCountsDto? Rate101 { get; set; }
    }

    public class FuzzyCountsDto
    {
        [Display("Segments")] public int Segments { get; set; }
        [Display("Words")] public int Words { get; set; }
        [Display("No match")] public int NoMatch { get; set; }
        [Display("50–74%")] public int Rate50_74 { get; set; }
        [Display("75–84%")] public int Rate75_84 { get; set; }
        [Display("85–94%")] public int Rate85_94 { get; set; }
        [Display("95–99%")] public int Rate95_99 { get; set; }
        [Display("100%")] public int Rate100 { get; set; }
    }
}
