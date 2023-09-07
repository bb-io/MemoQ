using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.Files.Requests;

public class GetAnalysisForProjectRequest : ProjectRequest
{
    [DataSource(typeof(FileFormatDataHandler))]
    public string? Format { get; set; }

    [DataSource(typeof(AnalysisAlgorithmDataHandler))]
    public string? Algorithm { get; set; }

    [Display("Homogeneity in analysis")] public bool? AnalysisHomogenity { get; set; }

    [Display("Analyze project translation memories")]
    public bool? AnalysisProjectTMs { get; set; }

    [Display("Analyze details by translation memory")]
    public bool? AnalysisDetailsByTm { get; set; }

    [Display("Disable cross-file repetition")]
    public bool? DisableCrossFileRepetition { get; set; }

    [Display("Include locked rows in analysis")]
    public bool? IncludeLockedRows { get; set; }

    [Display("Repetition preference over 100")]
    public bool? RepetitionPreferenceOver100 { get; set; }

    [Display("Show counts in analysis")] public bool? ShowCounts { get; set; }

    [Display("Show counts (include target count)")]
    public bool? ShowCountsIncludeTargetCount { get; set; }

    [Display("Show counts (include whitespaces in char count)")]
    public bool? ShowCountsIncludeWhitespacesInCharCount { get; set; }

    [Display("Show counts (status report)")]
    public bool? ShowCountsStatusReport { get; set; }

    [Display("Show results per file")] public bool? ShowResultsPerFile { get; set; }

    [Display("Tag character weight")] public double? TagCharWeight { get; set; }

    [Display("Tag word weight")] public double? TagWordWeight { get; set; }
}