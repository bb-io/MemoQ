using Blackbird.Applications.Sdk.Common;
using MQS.ServerProject;

namespace Apps.MemoQ.Models.Dto;

public class UserAssignmentDto
{
    [Display("Assignment type")]
    public string AssignmentType { get; set; }

    public string Role { get; set; }

    [Display("User ID")]
    public string UserId { get; set; }

    [Display("User name")]
    public string UserName { get; set; }

    public DateTime Deadline { get; set; }

    public UserAssignmentDto(TranslationDocumentDetailedSingleUserAssignmentInfo assignment)
    {
        AssignmentType = assignment.AssignmentType.ToString();
        Role = assignment.RoleId switch
        {
            0 => "Translator",
            1 => "Reviewer 1",
            2 => "Reviewer 2",
            _ => assignment.RoleId.ToString(),
        };
        UserId = assignment.User.AssigneeGuid.ToString();
        UserName = assignment.User.AssigneeName;
        Deadline = assignment.Deadline;
    }
}
