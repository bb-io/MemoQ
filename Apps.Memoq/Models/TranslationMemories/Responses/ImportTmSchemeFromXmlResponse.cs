using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.TranslationMemories.Responses;

public class ImportTmSchemeFromXmlResponse
{
    [Display("Conflicted fields")]
    public IEnumerable<string> ConflictedFields { get; set; }
}