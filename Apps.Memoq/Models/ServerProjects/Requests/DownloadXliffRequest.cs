using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.ServerProjects.Requests
{
    public class DownloadXliffRequest : ProjectRequest
    {
        [Display("Document GUID")]
        public string DocumentGuid { get; set; }

        [Display("Include full version history?")]
        public bool? FullVersionHistory { get; set; }

        [Display("Include skeleton?")]
        public bool? IncludeSkeleton { get; set; }

        [Display("Save compressed?")]
        public bool? SaveCompressed { get; set; }

        [Display("Use mqxliff")]
        public bool? UseMqxliff { get; set; }
    }
}
