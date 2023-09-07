using Apps.Memoq.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.ServerProjects.Requests;

public class CreateProjectRequest
{
    [Display("Source language")]
    [DataSource(typeof(SourceLanguageDataHandler))]
    public string SourceLangCode { get; set; }

    [Display("Target language")]
    [DataSource(typeof(TargetLanguageDataHandler))]
    public string TargetLangCode { get; set; }

    [Display("Project name")]
    public string ProjectName { get; set; }

    public DateTime Deadline { get; set; }

    [Display("Callback URL")]
    public string? CallbackUrl { get; set; }
    
    [Display("Disable MT plugins")]
    public bool? DisableMtPlugins { get; set; }

    [Display("Disable TB plugins")]
    public bool? DisableTbPlugins { get; set; }

    [Display("Disable TM plugins")]
    public bool? DisableTmPlugins { get; set; }

    [Display("Create offline TM/TB copies")]
    public bool? CreateOfflineTmtbCopies { get; set; }

    [Display("Download preview")]
    public bool? DownloadPreview { get; set; }

    [Display("Download skeleton")]
    public bool? DownloadSkeleton { get; set; }

    [Display("Enable split join")]
    public bool? EnableSplitJoin { get; set; }

    [Display("Enable web trans")]
    public bool? EnableWebTrans { get; set; }

    [Display("Allow overlapping workflow")]
    public bool? AllowOverlappingWorkflow { get; set; }

    [Display("Allow package creation")]
    public bool? AllowPackageCreation { get; set; }

    [Display("Client")]
    public string? Client { get; set; }

    [Display("Custom metadata")]
    public string? CustomMetas { get; set; }

    [Display("Description")]
    public string? Description { get; set; }

    [Display("Domain")]
    public string? Domain { get; set; }

    [Display("Download preview 2")]
    public bool? DownloadPreview2 { get; set; }

    [Display("Download skeleton 2")]
    public bool? DownloadSkeleton2 { get; set; }

    [Display("Enable communication")]
    public bool? EnableCommunication { get; set; }

    [Display("Strict Sublanguage Matching")]
    public bool? StrictSubLangMatching { get; set; }

    [Display("Subject")]
    public string? Subject { get; set; }
}