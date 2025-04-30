using Blackbird.Applications.Sdk.Common.Blueprints;

namespace Blackbird.Applications.SDK.Blueprints
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BlueprintEventDefinitionAttribute : BaseBlueprintInvocableDefinitionAttribute<BlueprintEvent>, IBlueprintEventDefinition
    {
        public BlueprintEventDefinitionAttribute(BlueprintEvent blueprintItem) : base(blueprintItem)
        {
        }
    }
}
