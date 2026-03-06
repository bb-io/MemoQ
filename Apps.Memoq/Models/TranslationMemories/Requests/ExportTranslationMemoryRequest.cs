using Apps.Memoq.Models.TranslationMemories.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Models.TranslationMemories.Requests
{
    public class ExportTranslationMemoryRequest : TranslationMemoryRequest
    {
        [Display("File name")]
        public string? FileName { get; set; }
    }
}
