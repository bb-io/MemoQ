using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Memoq.Models.TranslationMemories.Requests;

public class ImportTmSchemeRequest : TranslationMemoryRequest
{
    public FileReference File { get; set; }
}