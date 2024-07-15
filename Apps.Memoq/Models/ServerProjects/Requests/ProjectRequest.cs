using Apps.Memoq.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.ServerProjects.Requests;

public class ProjectRequest
{
    [Display("Project GUID")]
    [DataSource(typeof(ProjectDataHandler))]
    public string ProjectGuid { get; set; }
}