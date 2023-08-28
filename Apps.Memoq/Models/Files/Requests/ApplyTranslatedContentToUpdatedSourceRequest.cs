using Apps.Memoq.DataSourceHandlers.Enums;
using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.Files.Requests;

public class ApplyTranslatedContentToUpdatedSourceRequest : ProjectRequest
{
    [Display("Document GUID")] public string DocumentGuid { get; set; }

    [Display("Work with context IDs")] public bool? WorkWithContextIds { get; set; }

    [Display("XTranslate scenario")]
    [DataSource(typeof(XTranslateScenarioDataHandler))]
    public string? XTranslateScenario { get; set; }

    [Display("Expected final state")]
    [DataSource(typeof(ExpectedFinalStateDataHandler))]
    public string? ExpectedFinalState { get; set; }

    [Display("Insert empty translations")]  public bool? InsertEmptyTranslations { get; set; }

    [Display("Lock XTranslated rows")]  public bool? LockXTranslatedRows { get; set; }

    [Display("Source filter")]  
    [DataSource(typeof(SourceFilterDataHandler))]
    public string? SourceFilter { get; set; }
}