using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Files.Requests;

public class ListProjectFilesRequest
{
    [Display("Fill in assignment information")]
    public bool? FillInAssignmentInformation { get; set; }
}