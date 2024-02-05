using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Termbases.Responses;

public class ImportTermbaseResponse
{
    [Display("Termbase GUID")]
    public string TermbaseGuid { get; set; }
}