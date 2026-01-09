using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Callbacks.Models.Response
{
    public class AllFilesDeliveredDocumentDto
    {
        [Display("Document ID")]
        public string Id { get; set; } = default!;

        [Display("Name")]
        public string Name { get; set; } = default!;

        [Display("Status")]
        public string Status { get; set; } = default!;

        [Display("Target language")]
        public string? TargetLanguage { get; set; }
    }
}
