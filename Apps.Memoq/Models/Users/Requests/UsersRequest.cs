using Apps.Memoq.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.Users.Requests;

public class UsersRequest
{
    [Display("Users")]
    [DataSource(typeof(UserDataHandler))]
    public IEnumerable<string> UserGuids { get; set; }
}