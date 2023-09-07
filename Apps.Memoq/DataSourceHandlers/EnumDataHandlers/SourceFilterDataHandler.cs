using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class SourceFilterDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"ProofreadOnly", "Proofread only"},
        {"ProofreadOrConfirmed", "Proofread or confirmed"},
        {"AllTarget", "All target"},
    };
}