using Apps.Memoq.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.TranslationMemories.Requests;

public class AddTranslationMemoryToProjectRequest
{
    [Display("Translation memory GUID"), DataSource(typeof(TranslationMemoryDataHandler))]
    public IEnumerable<string> TmGuids { get; set; }

    [Display("Target language code"), DataSource(typeof(TargetLanguageDataHandler))]
    public string TargetLanguageCode { get; set; }

    [Display("Master translation memory GUID"), DataSource(typeof(TranslationMemoryDataHandler))]
    public string? MasterTmGuid { get; set; }

    [Display("Primary translation memory GUID"), DataSource(typeof(TranslationMemoryDataHandler))]
    public string? PrimaryTmGuid { get; set; }
}