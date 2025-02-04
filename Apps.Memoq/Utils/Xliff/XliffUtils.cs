using Blackbird.Applications.Sdk.Common.Exceptions;
using System.Xml.Linq;

namespace Apps.Memoq.Utils.Xliff;

public static class XliffUtils
{
    public static string GetXliffVersion(this XDocument document)
    {
        if (document?.Root == null)
        {
            throw new PluginMisconfigurationException("Invalid XLIFF document: does not have a root element");
        }

        var versionAttr = document.Root.Attribute("version");
        if (versionAttr == null)
        {
            throw new PluginMisconfigurationException("The XLIFF file's version attribute is missing.");
        }

        return versionAttr.Value.Trim();
    }
}