using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Files.Requests;

public class UpdateFromXliffRequest
{
    [Display("Re-import file ID", Description = "By providing a file ID, this action will overwrite the XLIFF")] 
    public string DocumentGuid { get; set; }

    [Display("Update locked segments")]
    public bool? UpdateLocked { get; set; }

    [Display("Update confirmed segments")]
    public bool? UpdateConfirmed { get; set; }
}