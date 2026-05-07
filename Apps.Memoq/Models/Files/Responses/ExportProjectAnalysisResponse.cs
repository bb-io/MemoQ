using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.MemoQ.Models.Files.Responses;

public record ExportProjectAnalysisResponse(FileReference ExportedAnalysis)
{
    [Display("Analysis file")] 
    public FileReference ExportedAnalysis { get; set; } = ExportedAnalysis;
}