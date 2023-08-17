using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.ServerProjects.Requests
{
    public class DownloadFileRequest : ProjectRequest
    {
        [Display("File GUID")]
        public string FileGuid { get; set; }
    }
}
