using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.Files.Requests
{
    public class ApplyTranslatedContentToUpdatedSourceRequest
    {
        public string ProjectGuid { get; set; }
        public string DocumentGuid { get; set; }
        public string SourceLangCode { get; set; }
        public string TargetLangCode { get; set; }
        public string Content { get; set; }
    }
}
