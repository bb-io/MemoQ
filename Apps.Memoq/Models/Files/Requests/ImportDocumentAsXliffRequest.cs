using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Files.Requests;

public class ImportDocumentAsXliffRequest
{
    [Display("Re-import file ID", Description = "By providing a file, this action will overwrite the XLIFF")] 
    public string? DocumentGuid { get; set; }

    [Display("Keep user assignments")]
    public bool? KeepUserAssignments { get; set; }

    [Display("Path to set as import path")]
    public string? PathToSetAsImportPath { get; set; }

    [Display("Update segment statuses")]
    public bool? UpdateSegmentStatuses { get; set; }
}