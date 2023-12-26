using Apps.Memoq.Models.Dto;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Memoq.Models.Files.Responses;

public class DownloadFileResponse : FileDto
{
    public DownloadFileResponse(FileDto file) : base(file)
    {
    }

    [Display("Document")] public FileReference File { get; set; }
}