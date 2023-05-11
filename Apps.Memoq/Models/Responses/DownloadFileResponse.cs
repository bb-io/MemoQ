using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.Responses
{
    public class DownloadFileResponse
    {
        public string FileName { get; set; }

        public byte[] File { get; set; }
    }
}
