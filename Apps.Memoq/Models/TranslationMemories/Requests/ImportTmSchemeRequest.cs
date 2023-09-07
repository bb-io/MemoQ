using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Memoq.Models.TranslationMemories.Requests;

public class ImportTmSchemeRequest : TranslationMemoryRequest
{
    public File File { get; set; }
}