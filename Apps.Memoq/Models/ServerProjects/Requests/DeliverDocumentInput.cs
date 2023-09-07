using Apps.Memoq.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.ServerProjects.Requests;

public class DeliverDocumentInput
{
    [Display("Delivering user")]
    [DataSource(typeof(UserDataHandler))]
    public string DeliveringUserGuid { get; set; }
    
    [Display("Document GUID")]
    public string DocumentGuid { get; set; }
    
    [Display("Return document to previous actor")]
    public bool? ReturnDocToPreviousActorField { get; set; }
}