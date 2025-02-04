using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Memoq.Models.Files.Requests;

public class OverwriteFileInProjectRequest : ProjectRequest
{
    [Display("Replacement file ID")]
    public string DocumentToReplaceGuid { get; set; }
        
    public FileReference File { get; set; }

    [Display("File name")]
    public string? Filename { get; set; }

    [Display("Path to set as import path")]
    public string? PathToSetAsImportPath { get; set; }

    [Display("Keep assignments")]
    public bool? KeepAssignments { get; set; }
}