using Blackbird.Applications.Sdk.Common.Blueprints;

namespace Blackbird.Applications.SDK.Blueprints
{
    public abstract class BaseBlueprintInvocableDefinitionAttribute<TBlueprintItem>(TBlueprintItem blueprintItem)
        : Attribute, IBaseBlueprintInvocableDefinition
        where TBlueprintItem : Enum
    {
        public TBlueprintItem BlueprintItem { get; } = blueprintItem;
    }
}
