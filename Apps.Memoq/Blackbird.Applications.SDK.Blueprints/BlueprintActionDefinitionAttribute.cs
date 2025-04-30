using Blackbird.Applications.Sdk.Common.Blueprints;

namespace Blackbird.Applications.SDK.Blueprints
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BlueprintActionDefinitionAttribute : BaseBlueprintInvocableDefinitionAttribute<BlueprintAction>, IBlueprintActionDefinition
    {
        public BlueprintActionDefinitionAttribute(BlueprintAction blueprintItem) : base(blueprintItem)
        {
        }
    }
}
