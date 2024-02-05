using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Termbases.Requests;

public class CreateTermbaseRequest
{
    [Display("Is QTerm")]
    public bool? IsQTerm { get; set; }
    
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public string? Client { get; set; }
    
    public string? Project { get; set; }
    
    public string? Domain { get; set; }
    
    public string? Subject { get; set; }
    
    [Display("Is moderated")]
    public bool? IsModerated { get; set; }
    
    [Display("Late disclosure", Description = "Can be applied only when 'Is moderated' parameter set to 'True'. When set " +
                                              "to 'False', entries appear immediately in a moderated term base. If the " +
                                              "terminologist rejects one later, it will be removed from the term base.")]
    public bool? ModLateDisclosure { get; set; }
}