using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Files.Requests;

public class UpdateFromXliffRequest
{
    [Display("Re-import document GUID", Description = "By providing a document, this action will overwrite the XLIFF")] 
    public string DocumentGuid { get; set; }
}