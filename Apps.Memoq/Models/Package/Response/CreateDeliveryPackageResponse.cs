using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Package.Response;

public class CreateDeliveryPackageResponse
{
    [Display("File ID")]
    public string FileGuid { get; set; }
    
    [Display("Not finished file IDs")]
    public IEnumerable<string> DocumentsNotFinished { get; set; }
}