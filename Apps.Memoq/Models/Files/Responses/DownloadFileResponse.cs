using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Files.Responses
{
    public class DownloadFileResponse
    {
        [Display("File name")]
        public string FileName { get; set; }

        public byte[] File { get; set; }
    }
}
