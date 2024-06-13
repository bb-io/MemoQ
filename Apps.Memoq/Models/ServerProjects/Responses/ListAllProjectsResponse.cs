using Apps.Memoq.Models.Dto;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.ServerProjects.Responses;

public class ListAllProjectsResponse
{
    [Display("Projects")]
    public List<ProjectDto> ServerProjects { get; set; }
}