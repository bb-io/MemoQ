using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Files.Responses;

public class UploadFileResponse
{
    [Display("Document GUIDs")]
    public IEnumerable<string> DocumentGuids { get; set; }
}