using Blackbird.Applications.Sdk.Common;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Memoq.Models.Files.Responses;

public class DownloadFileResponse
{
    [Display("Document")]
    public File File { get; set; }
}