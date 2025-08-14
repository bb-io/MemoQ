using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Models.ServerProjects.Responses
{
    public class LevenshteinEditDistanceReportResponse
    {
        [Display("Report ID")] public string? ReportId { get; set; }
        [Display("Total")] public LevenshteinCountsDto? Total { get; set; }
        [Display("By language")] public List<LevenshteinLanguageDto>? ByLanguage { get; set; }
    }

    public class LevenshteinLanguageDto
    {
        [Display("Language code")] public string? LanguageCode { get; set; }
        [Display("Documents")] public List<LevenshteinDocumentDto>? Documents { get; set; }
    }

    public class LevenshteinDocumentDto
    {
        [Display("Document ID")] public string? DocumentId { get; set; }
        [Display("Document name")] public string? DocumentName { get; set; }
        [Display("Average match rate")] public double? AverageMatchRate { get; set; }
        [Display("Normalized edit distance (doc)")] public double? NormalizedEditDistance { get; set; }

        [Display("Total")] public LevenshteinCountsDto? Total { get; set; }
        [Display("No match inserted")] public LevenshteinCountsDto? NoMatchInserted { get; set; }
        [Display("Machine translated")] public LevenshteinCountsDto? MachineTranslated { get; set; }
        [Display("X-translated")] public LevenshteinCountsDto? XTranslated { get; set; }
        [Display("50–74%")] public LevenshteinCountsDto? Rate50_74 { get; set; }
        [Display("75–84%")] public LevenshteinCountsDto? Rate75_84 { get; set; }
        [Display("85–94%")] public LevenshteinCountsDto? Rate85_94 { get; set; }
        [Display("95–99%")] public LevenshteinCountsDto? Rate95_99 { get; set; }
        [Display("100%")] public LevenshteinCountsDto? Rate100 { get; set; }
        [Display("101%")] public LevenshteinCountsDto? Rate101 { get; set; }
    }

    public class LevenshteinCountsDto
    {
        [Display("Segments")] public int Segments { get; set; }
        [Display("Words")] public int Words { get; set; }
        [Display("Edited segments")] public int EditedSegments { get; set; }
        [Display("Absolute edit distance")] public int AbsoluteEditDistance { get; set; }
        [Display("Normalized edit distance")] public double NormalizedEditDistance { get; set; }
    }
}
