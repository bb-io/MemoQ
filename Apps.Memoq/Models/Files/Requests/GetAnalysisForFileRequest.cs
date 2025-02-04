using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Files.Requests;

public class GetAnalysisForFileRequest : GetAnalysisForProjectRequest
{
    [Display("File ID")] public string DocumentGuid { get; set; }
}