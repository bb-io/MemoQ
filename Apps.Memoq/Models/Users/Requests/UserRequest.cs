using Apps.Memoq.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.Users.Requests;

public class UserRequest
{
    [Display("User")]
    [DataSource(typeof(UserDataHandler))]
    public string UserGuid { get; set; }
}