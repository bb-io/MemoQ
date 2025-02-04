using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.ServerProjects.Requests;

public class DownloadFileRequest : ProjectRequest
{
    [Display("File ID")]
    public string DocumentGuid { get; set; }

    [Display("Copy source to empty target?")]
    public bool? CopySourceToEmptyTarget { get; set; }

    [Display("Copy source to unconfirmed rows?")]
    public bool? CopySourceToUnconfirmedRows { get; set; }

    [Display("Export all multilingual siblings?")]
    public bool? ExportAllMultilingualSibling { get; set; }

    [Display("Revert faulty targets to source?")]
    public bool? RevertFaultyTargetsToSource { get; set; }
}