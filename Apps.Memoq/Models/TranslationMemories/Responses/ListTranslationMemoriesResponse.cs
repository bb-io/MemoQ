
using MQS.TM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.TranslationMemories.Responses
{
    public class ListTranslationMemoriesResponse
    {
        public IEnumerable<TMInfo> TranslationMemories { get; set; }
    }
}
