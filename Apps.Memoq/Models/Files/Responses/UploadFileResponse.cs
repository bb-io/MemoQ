using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Files.Responses;

public class UploadFileResponse
{
    [Display("File IDs")]
    public IEnumerable<string> DocumentGuids { get; set; }
}