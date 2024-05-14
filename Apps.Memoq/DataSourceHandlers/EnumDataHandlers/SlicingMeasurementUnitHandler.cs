using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class SlicingMeasurementUnitHandler : IStaticDataSourceHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        { "Characters", "Characters" },
        { "Words", "Words" },
        { "Segments", "Segments" },
    };

    public Dictionary<string, string> GetData()
    {
        return EnumValues;
    }
}