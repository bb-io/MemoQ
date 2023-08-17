using Apps.Memoq.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models;

public class LanguagesRequest
{
    [Display("Source language")]
    [DataSource(typeof(SourceLanguageDataHandler))]   
    public string SourceLanguage { get; set; }
    
    [Display("Target language")]
    [DataSource(typeof(TargetLanguageDataHandler))]  
    public string TargetLanguage { get; set; }
}