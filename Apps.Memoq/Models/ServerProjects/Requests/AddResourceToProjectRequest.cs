using Apps.Memoq.DataSourceHandlers;
using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.ServerProjects.Requests;

public class AddResourceToProjectRequest
{
    [Display("Resource type"), DataSource(typeof(ResourceTypeDataHandler))]
    public string ResourceType { get; set; }
    
    [Display("Resources"), DataSource(typeof(ResourceDataHandler))]
    public IEnumerable<string> ResourceGuids { get; set; }

    [Display("Object ids")]
    public IEnumerable<string> ObjectIds { get; set; }
}