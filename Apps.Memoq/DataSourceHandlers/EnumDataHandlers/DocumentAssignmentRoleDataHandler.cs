using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class DocumentAssignmentRoleDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"0", "Translator"},
        {"1", "Reviewer 1"},
        {"2", "Reviewer 2"},
    };
}