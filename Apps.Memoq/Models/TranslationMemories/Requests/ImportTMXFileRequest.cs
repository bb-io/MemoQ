using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Memoq.Models.TranslationMemories.Requests;

public class ImportTmxFileRequest
{
    [Display("Translation memory ID")]
    public string TmGuid { get; set; }

    public FileReference File { get; set; }
}