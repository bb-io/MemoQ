using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.TranslationMemories.Requests;

public class TranslationMemoryRequest
{
    [Display("Translation memory ID")]
    public string TmGuid { get; set; }
}