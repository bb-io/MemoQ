using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class DocumentAssignmentRoleDataHandler : IStaticDataSourceHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"0", "Translator"},
        {"1", "Reviewer 1"},
        {"2", "Reviewer 2"},
    };

    public Dictionary<string, string> GetData()
    {
        return EnumValues;
    }
}