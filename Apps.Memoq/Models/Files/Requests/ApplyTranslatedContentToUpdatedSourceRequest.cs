using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.Models.Files.Requests;

public class ApplyTranslatedContentToUpdatedSourceRequest : ProjectRequest
{
    [Display("File ID")] public string DocumentGuid { get; set; }

    [Display("Work with context IDs")] public bool? WorkWithContextIds { get; set; }

    [Display("XTranslate scenario")]
    [StaticDataSource(typeof(XTranslateScenarioDataHandler))]
    public string? XTranslateScenario { get; set; }

    [Display("Expected final state")]
    [StaticDataSource(typeof(ExpectedFinalStateDataHandler))]
    public string? ExpectedFinalState { get; set; }

    [Display("Insert empty translations")]  public bool? InsertEmptyTranslations { get; set; }

    [Display("Lock XTranslated rows")]  public bool? LockXTranslatedRows { get; set; }

    [Display("Source filter")]  
    [StaticDataSource(typeof(SourceFilterDataHandler))]
    public string? SourceFilter { get; set; }
}