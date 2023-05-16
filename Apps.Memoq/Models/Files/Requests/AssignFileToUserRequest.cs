using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.Files.Requests
{
    public class AssignFileToUserRequest
    {
        public string ProjectGuid { get; set; }
        public string FileGuid { get; set; }
        public string UserGuid { get; set; }
        public string Deadline { get; set; }
        public int Role { get; set; }
    }
}
