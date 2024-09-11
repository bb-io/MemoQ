using Apps.Memoq.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.Termbases.Requests;

public class TermbaseRequest
{
    [Display("Termbase ID"), DataSource(typeof(TermbaseDataHandler))]
    public string TermbaseId { get; set; }
}