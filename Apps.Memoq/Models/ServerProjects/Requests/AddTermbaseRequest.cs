using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Apps.Memoq.Models.Termbases.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MemoQ.Models.ServerProjects.Requests;

public class AddTermbaseRequest : TermbaseRequest
{
    [Display("Target language code"), StaticDataSource(typeof(TargetLanguageDataHandler))]
    public string TargetLanguageCode { get; set; } = string.Empty;
    
    [Display("Target termbase for new terms"), DataSource(typeof(TermbaseDataHandler))]
    public string? TargetTermbaseId { get; set; } 

    [Display("Exclude term bases from QA"), DataSource(typeof(TermbaseDataHandler))]
    public IEnumerable<string>? ExcludeTermBasesFromQa { get; set; }
}