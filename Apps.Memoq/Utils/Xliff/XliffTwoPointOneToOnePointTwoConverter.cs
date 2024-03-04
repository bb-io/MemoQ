using System.Xml.Linq;

namespace Apps.Memoq.Utils.Xliff;

public static class XliffTwoPointOneToOnePointTwoConverter
{
    public static XDocument ConvertToTwoPointOne(this XDocument document)
    {
        // Define namespaces
        XNamespace xliff12Ns = "urn:oasis:names:tc:xliff:document:1.2";
        XNamespace xliff21Ns = "urn:oasis:names:tc:xliff:document:2.0";

        // Check for the root element and its 'version' attribute
        if (document?.Root == null)
        {
            throw new InvalidOperationException("Document does not have a root element.");
        }

        var srcLang =
            document.Root.Elements(xliff12Ns + "file").Attributes("source-language").FirstOrDefault()?.Value ?? "en";
        var trgLang =
            document.Root.Elements(xliff12Ns + "file").Attributes("target-language").FirstOrDefault()?.Value ?? "uk";

        // Create the root element for the new XLIFF 2.1 document
        var newRoot = new XElement(xliff21Ns + "xliff",
            new XAttribute("version", "2.1"),
            new XAttribute("srcLang", srcLang),
            new XAttribute("trgLang", trgLang),
            document.Root.Elements(xliff12Ns + "file")
                .Select(fileElement =>
                    ProcessFileElementTo21(fileElement, xliff21Ns, xliff12Ns)) // Adjusted to pass xliff12Ns
        );

        return new XDocument(newRoot);
    }

    private static XElement ProcessFileElementTo21(XElement fileElement, XNamespace xliff21Ns, XNamespace xliff12Ns)
    {
        var file = fileElement.Attribute("original")?.Value;

        // Note: Now passing xliff12Ns to ProcessTransUnitTo21 to correctly select elements
        var newFileElement = new XElement(xliff21Ns + "file",
            new XAttribute("original", file),
            fileElement.Element(xliff12Ns + "body")
                .Elements(xliff12Ns + "trans-unit")
                .Select(transUnit =>
                    ProcessTransUnitTo21(transUnit, xliff21Ns, xliff12Ns)) // Adjusted to pass xliff12Ns
        );

        return newFileElement;
    }

    private static XElement ProcessTransUnitTo21(XElement transUnit, XNamespace xliff21Ns, XNamespace xliff12Ns)
    {
        var unitId = transUnit.Attribute("id")?.Value ?? "u1";
        var newUnitElement = new XElement(xliff21Ns + "unit",
            new XAttribute("id", unitId),
            new XElement(xliff21Ns + "segment",
                new XElement(xliff21Ns + "source", transUnit.Element(xliff12Ns + "source")?.Nodes()),
                new XElement(xliff21Ns + "target", transUnit.Element(xliff12Ns + "target")?.Nodes())
            )
        );

        return newUnitElement;
    }
}