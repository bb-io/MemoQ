using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Package.Request;

public class CreateDeliveryPackageRequest : ProjectRequest
{
    [Display("Document IDs")]
    public IEnumerable<string> DocumentIds { get; set; }
    
    [Display("Return to previous actor")]
    public bool? ReturnToPreviousActor { get; set; }
}