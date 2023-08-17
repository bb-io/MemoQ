using Blackbird.Applications.Sdk.Common;
using MQS.Security;

namespace Apps.Memoq.Models.Dto;

public class UserDto
{
    [Display("GUID")] public string Guid { get; set; }

    [Display("User name")] public string UserName { get; set; }

    public string Address { get; set; }

    [Display("Email address")] public string EmailAddress { get; set; }

    [Display("Full name")] public string FullName { get; set; }

    public string Password { get; set; }

    public UserDto(UserInfo user)
    {
        Guid = user.UserGuid.ToString();
        UserName = user.UserName;
        Address = user.Address;
        EmailAddress = user.EmailAddress;
        FullName = user.FullName;
        Password = user.Password;
    }
}