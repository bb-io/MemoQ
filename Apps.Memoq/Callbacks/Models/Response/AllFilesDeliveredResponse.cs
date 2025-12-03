using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Callbacks.Models.Response
{
    public class AllFilesDeliveredResponse
    {
        [Display("Project ID")]
        public string ProjectId { get; set; } = default!;

        [Display("Documents")]
        public List<AllFilesDeliveredDocumentDto> Documents { get; set; } = new();
    }
}
