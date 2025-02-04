using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Files.Requests;

public class DeleteFileRequest : ProjectRequest
{
    [Display("File ID")]
    public string FileGuid { get; set; }
}