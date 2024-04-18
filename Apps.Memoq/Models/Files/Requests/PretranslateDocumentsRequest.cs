using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.Files.Requests;

public class PretranslateDocumentsRequest
{
    [Display("Document GUIDs")]
    public IEnumerable<string> DocumentGuids { get; set; }

    [Display("Lock")]
    public bool? LockPretranslated { get; set; }
    
    [Display("Confirm lock pretranslated"), StaticDataSource(typeof(ConfirmLockDataHandler))]
    public string? ConfirmLockPreTranslated { get; set; }
    
    [Display("Pretranslate lookup behavior"), StaticDataSource(typeof(PretranslateLookupBehaviorDataHandler))]
    public string? PretranslateLookupBehavior { get; set; }
    
    [Display("Use MT")] 
    public bool? UseMt { get; set; }
    
    [Display("Translation memories GUIDs")]
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
}