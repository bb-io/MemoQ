using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.ServerProjects.Requests;

public class ListProjectsRequest
{
    public string? Client { get; set; }
    public string? Domain { get; set; }

    [Display("Last changed before")] public DateTime? LastChangedBefore { get; set; }

    [Display("Name or description")] public string? NameOrDescription { get; set; }
    public string? Project { get; set; }

    [Display("Source language")]
    [DataSource(typeof(SourceLanguageDataHandler))]
    public string? SourceLanguageCode { get; set; }

    [Display("Target language"), StaticDataSource(typeof(TargetLanguageDataHandler))]
    public string? TargetLanguageCode { get; set; }

    [Display("Time closed")] public DateTime? TimeClosed { get; set; }
    public string? Subject { get; set; }
}