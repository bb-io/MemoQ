using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.TranslationMemories.Requests
{
    public class ImportTMXFileRequest
    {
        public string tmGuid { get; set; }

        public byte[] File { get; set; }
    }
}
