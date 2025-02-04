using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class SourceFilterDataHandler : IStaticDataSourceItemHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"ProofreadOnly", "Proofread only"},
        {"ProofreadOrConfirmed", "Proofread or confirmed"},
        {"AllTarget", "All target"},
    };

    public IEnumerable<DataSourceItem> GetData()
    {
        return EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}