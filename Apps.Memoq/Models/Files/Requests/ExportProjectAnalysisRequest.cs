using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.Models.Files.Requests;

public class ExportProjectAnalysisRequest
{
    [StaticDataSource(typeof(AnalysisAlgorithmDataHandler))]
    public string? Algorithm { get; set; }

    [Display("Homogeneity in analysis")] 
    public bool? AnalysisHomogeneity { get; set; }

    [Display("Disable cross-file repetition")]
    public bool? DisableCrossFileRepetition { get; set; }

    [Display("Include locked rows in analysis")]
    public bool? IncludeLockedRows { get; set; }

    [Display("Repetition preference over 100")]
    public bool? RepetitionPreferenceOver100 { get; set; }

    [Display("Show counts (include whitespaces in char count)")]
    public bool? ShowCountsIncludeWhitespacesInCharCount { get; set; }

    [Display("Tag character weight")] 
    public double? TagCharWeight { get; set; }

    [Display("Tag word weight")] 
    public double? TagWordWeight { get; set; }
}