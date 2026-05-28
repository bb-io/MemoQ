using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.Callbacks.Models.Request;

public class OnFileDeliveredRequest
{
    [Display("Target language"), StaticDataSource(typeof(TargetLanguageDataHandler))]
    public string? TargetLanguage { get; set; }

    [Display("Project name contains")]
    public string? ProjectNameContains { get; set; }
}