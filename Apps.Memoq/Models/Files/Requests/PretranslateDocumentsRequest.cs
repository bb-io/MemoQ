using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.Files.Requests;

public class PretranslateDocumentsRequest
{
    [Display("Document GUIDs")]
    public IEnumerable<string> DocumentGuids { get; set; }

    [Display("Lock")]
    public bool? LockPretranslated { get; set; }
    
    [Display("Confirm lock pre translated"), DataSource(typeof(ConfirmLockDataHandler))]
    public string? ConfirmLockPreTranslated { get; set; }
    
    [Display("Pretranslate lookup behavior"), DataSource(typeof(PretranslateLookupBehaviorDataHandler))]
    public string? PretranslateLookupBehavior { get; set; }
    
    [Display("Use MT")] 
    public bool? UseMt { get; set; }
}