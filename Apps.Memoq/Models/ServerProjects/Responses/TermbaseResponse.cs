using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.MemoQ.Models.ServerProjects.Responses;
public class TermbaseResponse
{
    [Display("Term base IDs")]
    public IEnumerable<string> TermbaseIds { get; set; }
}
