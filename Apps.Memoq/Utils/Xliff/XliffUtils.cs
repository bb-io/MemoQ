using System.Xml.Linq;

namespace Apps.Memoq.Utils.Xliff;

public static class XliffUtils
{
    public static string GetXliffVersion(this XDocument document)
    {
        if (document?.Root == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        var versionAttr = document.Root.Attribute("version");
        if (versionAttr == null)
        {
            throw new InvalidOperationException("The XLIFF version attribute is missing.");
        }

        return versionAttr.Value.Trim();
    }
}