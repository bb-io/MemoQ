using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models;

public class LanguagesRequest
{
    [Display("Source language")]
    [DataSource(typeof(SourceLanguageDataHandler))]   
    public string SourceLanguage { get; set; }
    
    [Display("Target language"), StaticDataSource(typeof(TargetLanguageDataHandler))]  
    public string TargetLanguage { get; set; }
}