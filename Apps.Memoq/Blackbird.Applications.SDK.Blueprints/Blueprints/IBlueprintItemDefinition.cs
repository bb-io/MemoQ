namespace Blackbird.Applications.Sdk.Common.Blueprints;

public interface IBlueprintItemDefinition
{
    public string IconPath { get; }
    public string Description { get; }
    public string? DefaultCategory { get; }
}