using Blackbird.Applications.Sdk.Common;
using MQS.Security;

namespace Apps.Memoq.Models.Dto;

public class GroupDto
{
    [Display("GUID")] public string Guid { get; set; }

    [Display("Name")] public string Name { get; set; }

    [Display("Description")] public string Description { get; set; }

    [Display("Is disabled")] public bool IsDisabled { get; set; }

    [Display("Is subvendor group")] public bool IsSubvendorGroup { get; set; }

    public GroupDto(GroupInfo group)
    {
        Guid = group.GroupGuid.ToString();
        Name = group.GroupName;
        Description = group.Description;
        IsDisabled = group.IsDisabled;
        IsSubvendorGroup = group.IsSubvendorGroup;
    }
}