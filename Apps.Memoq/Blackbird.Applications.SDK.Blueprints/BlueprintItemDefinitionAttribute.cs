using Blackbird.Applications.Sdk.Common.Blueprints;

namespace Blackbird.Applications.SDK.Blueprints
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class BlueprintItemDefinitionAttribute : Attribute, IBlueprintItemDefinition
    {
        public string IconPath { get; }
        public string Description { get; }
        public string? DefaultCategory { get; set; }

        public BlueprintItemDefinitionAttribute(string iconPath, string description)
        {
            IconPath = iconPath;
            Description = description;
        }
    }
}
