using Apps.Memoq.DataSourceHandlers;
using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.Files.Requests;

public class AssignFileToUserRequest : ProjectRequest
{
    [Display("File GUID")]
    public string FileGuid { get; set; }
        
    [Display("User")]
    [DataSource(typeof(UserDataHandler))]
    public string UserGuid { get; set; }
    public DateTime Deadline { get; set; }
        
    [StaticDataSource(typeof(DocumentAssignmentRoleDataHandler))]
    public string? Role { get; set; }
}