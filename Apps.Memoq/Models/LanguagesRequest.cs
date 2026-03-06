using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models;

public class LanguagesRequest
{
    [Display("Source language")]
    [StaticDataSource(typeof(SourceLanguageDataHandler))]   
    public string SourceLanguage { get; set; }
    
    [Display("Target language"), StaticDataSource(typeof(TargetLanguageDataHandler))]  
    public string TargetLanguage { get; set; }

    [Display("Name or description")]
    public string? NameOrDescription { get; set; }

    public string? Client { get; set; }

    public string? Domain { get; set; }

    public int? Priority { get; set; }

    [Display("Last modified after")]
    public DateTime? LastModifiedAfter { get; set; }

    [Display("Last modified before")]
    public DateTime? LastModifiedBefore { get; set; }

    [Display("Last used after")]
    public DateTime? LastUsedAfter { get; set; }

    [Display("Last used before")]
    public DateTime? LastUsedBefore { get; set; }

    public string? Subject { get; set; }

    [Display("Project")]
    public string? Project { get; set; }

    [Display("Used in project")]
    public bool? UsedInProject { get; set; }
}