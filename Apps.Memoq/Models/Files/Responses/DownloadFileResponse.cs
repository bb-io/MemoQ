using Apps.MemoQ.Models.Dto;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Memoq.Models.Files.Responses;

public class DownloadFileResponse : FileInfoDto
{
    public DownloadFileResponse(FileInfoDto file) : base(file)
    {
        DocumentName = file.Name + file.FileExtension;
    }

    [Display("File")] public FileReference File { get; set; }

    [Display("File name")] public string DocumentName { get; set; }
}