using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Memoq.Models.Files.Requests;

public class OverwriteFileInProjectRequest : ProjectRequest
{
    [Display("Document to replace GUID")]
    public string DocumentToReplaceGuid { get; set; }
        
    public File File { get; set; }

    [Display("File name")]
    public string? Filename { get; set; }

    [Display("Path to set as import path")]
    public string? PathToSetAsImportPath { get; set; }

    [Display("Keep assignments")]
    public bool? KeepAssignments { get; set; }
}