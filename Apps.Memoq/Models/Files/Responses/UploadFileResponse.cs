using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Files.Responses;

public class UploadFileResponse
{
    [Display("Document GUID")]
    public string DocumentGuid { get; set; }
}