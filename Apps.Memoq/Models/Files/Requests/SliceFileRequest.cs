using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.Files.Requests;

public class SliceFileRequest
{
    [Display("Document GUID")]
    public string DocumentGuid { get; set; }
    
    [Display("Slicing measurement unit")]
    [DataSource(typeof(SlicingMeasurementUnitHandler))]
    public string SlicingMeasurementUnit { get; set; }
    
    [Display("Number of parts")]
    public int NumberOfParts { get; set; }
}