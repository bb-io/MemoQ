using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.TranslationMemories.Responses;

public class ImportTmxFileResponse
{
    [Display("GUID")]
    public string Guid { get; set; }
}