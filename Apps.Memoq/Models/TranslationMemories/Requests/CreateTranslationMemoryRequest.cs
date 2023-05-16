using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.TranslationMemories.Requests
{
    public class CreateTranslationMemoryRequest
    {
        public string Name { get; set; }

        public string SourceLanguageCode { get; set; }

        public string TargetLanguageCode { get; set; }
    }
}
