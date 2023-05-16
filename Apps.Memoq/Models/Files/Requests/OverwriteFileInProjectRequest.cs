using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.Files.Requests
{
    public class OverwriteFileInProjectRequest
    {
        public string ProjectGuid { get; set; }

        public string DocumentToReplaceGuid { get; set; }
        
        public byte[] File { get; set; }

        public string Filename { get; set; }

        public bool KeepAssignments { get; set; }
    }
}
