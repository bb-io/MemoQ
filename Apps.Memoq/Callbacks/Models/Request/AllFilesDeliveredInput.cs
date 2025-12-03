using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Callbacks.Models.Request
{
    public class AllFilesDeliveredInput
    {
        [Display("Project ID")]
        public string ProjectId { get; set; } = default!;
    }
}
