using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Package.Request;

public class PackageRequest
{
    [Display("File ID")]
    public string FileId { get; set; }
}