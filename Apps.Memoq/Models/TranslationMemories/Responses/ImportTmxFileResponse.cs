using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.TranslationMemories.Responses;

public class ImportTmxFileResponse
{
    [Display("File ID")]
    public string Guid { get; set; }
}