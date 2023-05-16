
using MQS.TasksService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.Files.Responses
{
    public class ApplyTranslatedContentToUpdatedSourceResponse
    {
        public IEnumerable<XTranslateDocumentResult> Results { get; set; }
    }
}
