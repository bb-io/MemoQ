using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class SourceFilterDataHandler : IStaticDataSourceHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"ProofreadOnly", "Proofread only"},
        {"ProofreadOrConfirmed", "Proofread or confirmed"},
        {"AllTarget", "All target"},
    };

    public Dictionary<string, string> GetData()
    {
        return EnumValues;
    }
}