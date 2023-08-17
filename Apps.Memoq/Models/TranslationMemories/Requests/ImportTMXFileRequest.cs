using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.TranslationMemories.Requests
{
    public class ImportTMXFileRequest
    {
        [Display("Translation memory GUID")]
        public string TmGuid { get; set; }

        public byte[] File { get; set; }
    }
}
