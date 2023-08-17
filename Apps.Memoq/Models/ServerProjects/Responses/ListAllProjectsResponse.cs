using Apps.Memoq.Models.Dto;

namespace Apps.Memoq.Models.ServerProjects.Responses
{
    public class ListAllProjectsResponse
    {
        public ProjectDto[] ServerProjects { get; set; }
    }
}
