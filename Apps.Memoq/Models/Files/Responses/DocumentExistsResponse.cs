using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Models.Files.Responses;

public class DocumentExistsResponse
{
    [Display("File exists")]
    public bool Result { get; set; }
}