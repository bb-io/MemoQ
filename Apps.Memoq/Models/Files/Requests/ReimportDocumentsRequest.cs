using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Files.Requests;

public class ReimportDocumentsRequest
{
    [Display("Document GUID")] 
    public string DocumentGuid { get; set; }

    [Display("Keep user assignments")]
    public bool? KeepUserAssignments { get; set; }

    [Display("Path to set as import path")]
    public string? PathToSetAsImportPath { get; set; }
}