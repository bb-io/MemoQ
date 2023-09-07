using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class ExpectedFinalStateDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"SameAsPrevious", "Same as previous"},
        {"Pretranslated", "Pretranslated"},
        {"Confirmed", "Confirmed"},
        {"Proofread", "Proofread"},
        {"Reviewer1Confirmed", "Reviewer 1 confirmed"},
    };
}