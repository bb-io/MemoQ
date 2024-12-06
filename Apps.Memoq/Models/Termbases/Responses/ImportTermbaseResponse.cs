using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Termbases.Responses;

public class ImportTermbaseResponse
{
    [Display("Termbase GUID", Description = "The unique identifier of the created or updated termbase.")]
    public string TermbaseGuid { get; set; }
}