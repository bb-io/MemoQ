using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.TranslationMemories.Requests;

public class UpdateTranslationMemoryPropertiesRequest
{
    public string? Name { get; set; }
    public string? Client { get; set; }
    public string? Domain { get; set; }
    public string? Subject { get; set; }
    public string? Project { get; set; }

    [Display("Optimization preference")]
    [StaticDataSource(typeof(OptimizationPreferenceDataHandler))]
    public string? OptimizationPreference { get; set; }

    [Display("Store document full path")] public bool? StoreDocumentFullPath { get; set; }

    [Display("Store document name")] public bool? StoreDocumentName { get; set; }
}