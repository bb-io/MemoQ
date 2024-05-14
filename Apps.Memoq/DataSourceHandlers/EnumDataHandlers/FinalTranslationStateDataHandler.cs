using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class FinalTranslationStateDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new Dictionary<string, string>
        {
            { "0", "No change" },
            { "1", "Confirmed" },
            { "2", "Proofread" },
            { "3", "Pretranslated" },
            { "4", "Reviewer 1 confirmed" }
        };
    }
}