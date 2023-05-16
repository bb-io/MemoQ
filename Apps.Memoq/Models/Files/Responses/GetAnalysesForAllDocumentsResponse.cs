using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.Files.Responses
{
    public class GetAnalysesForAllDocumentsResponse
    {
        public IEnumerable<GetAnalysisResponse> Analyses { get; set; }
    }
}
