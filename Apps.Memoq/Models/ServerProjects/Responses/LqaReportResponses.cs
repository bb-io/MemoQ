using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.MemoQ.Models.ServerProjects.Responses;

public class LqaStatisticsReportResponse
{
    [Display("Exported report")]
    public FileReference? ExportedReport { get; set; }

    [Display("Project statistics")]
    public LqaProjectStatisticsDto? ProjectStatistics { get; set; }

    [Display("Document statistics")]
    public IEnumerable<LqaDocumentStatisticsDto> DocumentStatistics { get; set; } = [];

    [Display("Model statistics")]
    public IEnumerable<LqaAggregatedStatisticsDto> ModelStatistics { get; set; } = [];
}

public class LqaRejectedSegmentsReportResponse
{
    [Display("Exported report")]
    public FileReference? ExportedReport { get; set; }

    [Display("Documents")]
    public IEnumerable<LqaRejectedDocumentDto> Documents { get; set; } = [];

    [Display("Model summaries")]
    public IEnumerable<LqaModelSummaryDto> ModelSummaries { get; set; } = [];
}

public class LqaProjectStatisticsDto
{
    [Display("Project name")]
    public string? ProjectName { get; set; }

    [Display("Number of files")]
    public int NumberOfFiles { get; set; }

    [Display("Number of reviewed files")]
    public int NumberOfReviewedFiles { get; set; }

    [Display("Number of passed files")]
    public int NumberOfPassedFiles { get; set; }

    [Display("Number of failed files")]
    public int NumberOfFailedFiles { get; set; }

    [Display("Number of LQA models")]
    public int NumberOfLqaModels { get; set; }
}

public class LqaDocumentStatisticsDto : LqaAggregatedStatisticsDto
{
    [Display("Document name")]
    public string? DocumentName { get; set; }

    [Display("Passed")]
    public bool Passed { get; set; }
}

public class LqaAggregatedStatisticsDto
{
    [Display("Model ID")]
    public string ModelId { get; set; } = string.Empty;

    [Display("Model name")]
    public string? ModelName { get; set; }

    [Display("Number of segments")]
    public int NumberOfSegments { get; set; }

    [Display("Number of words")]
    public int NumberOfWords { get; set; }

    [Display("Number of rejected segments")]
    public int NumberOfRejectedSegments { get; set; }

    [Display("Total number of errors")]
    public int TotalNumberOfErrors { get; set; }

    [Display("Total penalty points")]
    public int TotalPenaltyPoints { get; set; }

    [Display("Normalized penalty point")]
    public double NormalizedPenaltyPoint { get; set; }

    [Display("Errors by category")]
    public IEnumerable<LqaNamedIntValueDto> ErrorsByCategory { get; set; } = [];

    [Display("Errors by severity")]
    public IEnumerable<LqaNamedIntValueDto> ErrorsBySeverity { get; set; } = [];

    [Display("Normalized penalty points by severity")]
    public IEnumerable<LqaNamedDoubleValueDto> NormalizedPenaltyPointsBySeverity { get; set; } = [];

    [Display("Penalty points by category")]
    public IEnumerable<LqaPenaltyCategoryDto> PenaltyPointsByCategory { get; set; } = [];
}

public class LqaPenaltyCategoryDto
{
    [Display("Category")]
    public string? Category { get; set; }

    [Display("Total penalty")]
    public int TotalPenalty { get; set; }

    [Display("Penalty points by subcategory")]
    public IEnumerable<LqaNamedIntValueDto> PenaltyPointsBySubcategory { get; set; } = [];
}

public class LqaNamedIntValueDto
{
    [Display("Name")]
    public string Name { get; set; } = string.Empty;

    [Display("Value")]
    public int Value { get; set; }
}

public class LqaNamedDoubleValueDto
{
    [Display("Name")]
    public string Name { get; set; } = string.Empty;

    [Display("Value")]
    public double Value { get; set; }
}

public class LqaRejectedDocumentDto
{
    [Display("Document ID")]
    public string DocumentId { get; set; } = string.Empty;

    [Display("Document name")]
    public string? DocumentName { get; set; }

    [Display("LQA model name")]
    public string? LqaModelName { get; set; }

    [Display("Date")]
    public string? Date { get; set; }

    [Display("Reviewer")]
    public string? Reviewer { get; set; }

    [Display("Translator")]
    public string? Translator { get; set; }

    [Display("Result")]
    public string? Result { get; set; }

    [Display("Rejected segments")]
    public IEnumerable<LqaRejectedSegmentDto> RejectedSegments { get; set; } = [];
}

public class LqaRejectedSegmentDto
{
    [Display("Row number")]
    public int RowNumber { get; set; }

    [Display("Source segment")]
    public string? SourceSegment { get; set; }

    [Display("Target segment")]
    public string? TargetSegment { get; set; }

    [Display("Rejections")]
    public IEnumerable<LqaRejectionDto> Rejections { get; set; } = [];
}

public class LqaRejectionDto
{
    [Display("Comment")]
    public string? Comment { get; set; }

    [Display("Category")]
    public string? Category { get; set; }

    [Display("Subcategory")]
    public string? Subcategory { get; set; }

    [Display("Severity")]
    public string? Severity { get; set; }

    [Display("Original")]
    public string? Original { get; set; }

    [Display("Suggestion")]
    public string? Suggestion { get; set; }
}

public class LqaModelSummaryDto
{
    [Display("Model ID")]
    public string ModelId { get; set; } = string.Empty;

    [Display("Pass criteria")]
    public string? PassCriteria { get; set; }

    [Display("Documents")]
    public IEnumerable<string> Documents { get; set; } = [];

    [Display("Statistics")]
    public LqaAggregatedStatisticsDto? Statistics { get; set; }
}
