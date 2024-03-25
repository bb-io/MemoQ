using Apps.Memoq.DataSourceHandlers;
using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.ServerProjects.Requests;

// Docs about ObjectIds: https://docs.memoq.com/current/api-docs/wsapi/api/serverprojectservice/MemoQServices.ServerProjectResourceAssignmentForResourceType.html
public class AddResourceToProjectRequest
{
    [Display("Resource type"), DataSource(typeof(ResourceTypeDataHandler))]
    public string ResourceType { get; set; }
    
    [Display("Resource ID"), DataSource(typeof(ResourceDataHandler))]
    public string ResourceGuid { get; set; }

    [Display("Object IDs"), DataSource(typeof(ObjectDataHandler))]
    public IEnumerable<string>? ObjectIds { get; set; }

    public bool? Primary { get; set; }
}