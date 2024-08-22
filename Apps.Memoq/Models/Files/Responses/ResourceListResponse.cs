using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Models.Files.Responses
{
    public class ResourceListResponse
    {
        [Display("Resource IDs")]
        public IEnumerable<string> ResourceIds { get; set; }
    }
}
