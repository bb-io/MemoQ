using MQS.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.Users.Responses
{
    public class ListAllUsersResponse
    {
        public IEnumerable<UserInfo> Users { get; set; }
    }
}
