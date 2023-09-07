using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class SlicingMeasurementUnitHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "Characters", "Characters" },
        { "Words", "Words" },
        { "Segments", "Segments" },
    };
}