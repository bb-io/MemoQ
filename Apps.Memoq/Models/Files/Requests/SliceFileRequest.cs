using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.Models.Files.Requests;

public class SliceFileRequest
{
    [Display("File ID")]
    public string DocumentGuid { get; set; }
    
    [Display("Slicing measurement unit")]
    [StaticDataSource(typeof(SlicingMeasurementUnitHandler))]
    public string SlicingMeasurementUnit { get; set; }
    
    [Display("Number of parts")]
    public int NumberOfParts { get; set; }
}