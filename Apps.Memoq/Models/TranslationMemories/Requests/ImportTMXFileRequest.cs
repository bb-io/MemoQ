using Blackbird.Applications.Sdk.Common;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Memoq.Models.TranslationMemories.Requests;

public class ImportTmxFileRequest
{
    [Display("Translation memory GUID")]
    public string TmGuid { get; set; }

    public File File { get; set; }
}