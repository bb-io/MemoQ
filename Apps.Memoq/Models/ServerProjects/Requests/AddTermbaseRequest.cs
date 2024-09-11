using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Apps.Memoq.Models.Termbases.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MemoQ.Models.ServerProjects.Requests;

public class AddTermbaseRequest : TermbaseRequest
{
    [Display("Target language code", Description = "The three+(two) letter code of the target language of the project. (such as fre, eng, eng-US) for which the TBs are assigned to. Existing TB assignments for this project target language are deleted."), StaticDataSource(typeof(TargetLanguageDataHandler))]
    public string TargetLanguageCode { get; set; } = string.Empty;
    
    [Display("Target termbase for new terms", Description = "The Guid of the term base which is set as the target for new terms for the specified target language of the project."), DataSource(typeof(TermbaseDataHandler))]
    public string? TargetTermbaseId { get; set; } 

    [Display("Excluded term bases from QA", Description = "The Guids of the TBs assigned to the project (for the target language specified by TargetLangCode). The index of the items correspont to the rank of the term base. Each term base must contain the project source and the given project target language, when lazy match is performed."), DataSource(typeof(TermbaseDataHandler))]
    public IEnumerable<string>? ExcludeTermBasesFromQa { get; set; }
}