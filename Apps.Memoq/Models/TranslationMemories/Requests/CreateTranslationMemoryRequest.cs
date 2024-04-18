using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.TranslationMemories.Requests;

public class CreateTranslationMemoryRequest : LanguagesRequest
{
    public string Name { get; set; }
    public string? Client;
    public string? Domain;
    public string? Subject;
    public string? Project;

    [Display("Allow multiple")] public bool? AllowMultiple { get; set; }

    [Display("Allow reverse lookup")] public bool? AllowReverseLookup { get; set; }

    [Display("Creator username")] public string? CreatorUsername { get; set; }

    [Display("Optimization preference")]
    [StaticDataSource(typeof(OptimizationPreferenceDataHandler))]
    public string? OptimizationPreference { get; set; }

    [Display("Store document full path")] public bool? StoreDocumentFullPath { get; set; }

    [Display("Store document name")] public bool? StoreDocumentName { get; set; }

    [Display("Store formatting")] public bool? StoreFormatting { get; set; }

    [Display("TM engine type")]
    [StaticDataSource(typeof(TmEngineTypeDataHandler))]
    public string? TmEngineType { get; set; }

    [Display("Use context")] public bool? UseContext { get; set; }

    [Display("Use IceSpice context")] public bool? UseIceSpiceContext { get; set; }
}