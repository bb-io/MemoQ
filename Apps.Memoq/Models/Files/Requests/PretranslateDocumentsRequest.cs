using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.Models.Files.Requests;

public class PretranslateDocumentsRequest
{
    [Display("File IDs", Description = "If not provided, all files in the project will be pretranslated")]
    public IEnumerable<string>? DocumentGuids { get; set; }
    
    [Display("Target languages", Description = "If not provided, files with all target languages will be translated"), StaticDataSource(typeof(TargetLanguageDataHandler))]
    public IEnumerable<string>? TargetLanguages { get; set; }

    [Display("Lock", Description = "By default: true")]
    public bool? LockPretranslated { get; set; }
    
    [Display("Confirm lock pretranslated", Description = "By default: Exact match"), StaticDataSource(typeof(ConfirmLockDataHandler))]
    public string? ConfirmLockPreTranslated { get; set; }
    
    [Display("Pretranslate lookup behavior"), StaticDataSource(typeof(PretranslateLookupBehaviorDataHandler))]
    public string? PretranslateLookupBehavior { get; set; }
    
    [Display("Use MT", Description = "By default: true")] 
    public bool? UseMt { get; set; }
    
    [Display("Translation memories IDs")]
    public IEnumerable<string>? TranslationMemoriesGuids { get; set; }
    
    [Display("Include numbers", Description = "By default: true")]
    public bool? IncludeNumbers { get; set; }
    
    [Display("Change case", Description = "By default: false")]
    public bool? ChangeCase { get; set; }
    
    [Display("Include auto translations", Description = "By default: true")]
    public bool? IncludeAutoTranslations { get; set; }
    
    [Display("Include fragments", Description = "By default: true")]
    public bool? IncludeFragments { get; set; }
    
    [Display("Include non-translatables", Description = "By default: true")]
    public bool? IncludeNonTranslatables { get; set; }
    
    [Display("Include term bases", Description = "By default: true")]
    public bool? IncludeTermBases { get; set; }
    
    [Display("Minimum coverage", Description = "By default: 50")]
    public int? MinCoverage { get; set; }
    
    [Display("Coverage type", Description = "By default: Not full"), StaticDataSource(typeof(MatchCoverageTypeDataHandler))]
    public string? CoverageType { get; set; }
    
    [Display("Only unambiguous matches", Description = "By default: true")]
    public bool? OnlyUnambiguousMatches { get; set; }

    [Display("Final translation state", Description = "By default: No change"), StaticDataSource(typeof(FinalTranslationStateDataHandler))]
    public string? FinalTranslationState { get; set; }
}