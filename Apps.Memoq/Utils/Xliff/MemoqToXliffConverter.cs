using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Apps.Memoq.Utils.Xliff;

public static class MemoqToXliffConverter
{
    private static readonly XNamespace ns = "urn:oasis:names:tc:xliff:document:1.2";
    private static XNamespace _mqNs = "MQXliff";

        public static string ConvertMqXliffToXliff(this Stream inputStream, bool useSkeleton, bool ignoreChildrenInSource = true)
    {
        using var reader = new StreamReader(inputStream, Encoding.UTF8);
        var xliffDocument = XDocument.Load(reader);

        // Extract the default namespace from the .mqxliff file.
        XNamespace defaultNs = xliffDocument.Root.GetDefaultNamespace();

        var newXliff = new XDocument(
            new XElement(ns + "xliff", new XAttribute("version", "1.2"),
                new XElement(ns + "file",
                    new XAttribute("original", xliffDocument.Root?.Element(defaultNs + "file")?.Attribute("original")?.Value ?? "unknown"),
                    new XAttribute("source-language", xliffDocument.Root?.Element(defaultNs + "file")?.Attribute("source-language")?.Value ?? "unknown"),
                    new XAttribute("target-language", xliffDocument.Root?.Element(defaultNs + "file")?.Attribute("target-language")?.Value ?? "unknown"),
                    new XAttribute("datatype", xliffDocument.Root?.Element(defaultNs + "file")?.Attribute("datatype")?.Value ?? "plaintext"),
                    CreateHeader(xliffDocument, useSkeleton),
                    new XElement(ns + "body")
                // CreateBody(xliffDocument, defaultNs, ignoreChildrenInSource)
                )
            )
        );
        var tus = (from tu in xliffDocument.Descendants(ns + "trans-unit") select tu).ToList();
        string content = AddBody(tus, newXliff);
        return content;
    }

    private static XElement CreateHeader(XDocument xliffDocument, bool useSkeleton)
    {
        var header = new XElement(ns + "header");

        if (useSkeleton)
        {
            header.Add(new XElement(ns + "skl",
                new XElement(ns + "internal-file", new XAttribute("form", "text/xml"),
                    new XElement(ns + "xliff", new XAttribute("version", "1.2"),
                        xliffDocument.Elements()
                    )
                )
            ));
        }

        return header;
    }

    private static string AddBody(List<XElement> tus, XDocument newXliff)
    {
        string fileContent;
        Encoding encoding;
        var xliffStream = new MemoryStream();
        newXliff.Save(xliffStream);

        xliffStream.Position = 0;
        using (StreamReader inFileStream = new StreamReader(xliffStream))
        {
            encoding = inFileStream.CurrentEncoding;
            fileContent = inFileStream.ReadToEnd();
        }
        var body = new StringBuilder();
        foreach (var tu in tus)
        {
            var source = RemoveExtraNewLines(Regex.Replace(tu.Element(ns + "source").ToString(), @"</?source(.*?)>", @""));
            var target = RemoveExtraNewLines(Regex.Replace(tu.Element(ns + "target").ToString(), @"</?target(.*?)>", @""));
            var tuid = tu.Attribute("id").Value;
            body.Append($"  <trans-unit id=\"" + tuid + "\">" + Environment.NewLine + "\t\t<source>" + source + "</source>" + Environment.NewLine + "\t\t<target>" + target + "</target>" + Environment.NewLine + "	</trans-unit>" + Environment.NewLine);
        }
        fileContent = fileContent.Replace("<body />", body.ToString());
        return fileContent;

    }

    private static string RemoveExtraNewLines(string originalString)
    {
        if (!string.IsNullOrWhiteSpace(originalString))
        {
            var to_modify = originalString;
            to_modify = Regex.Replace(to_modify, @"(\r\n\s+)", "", RegexOptions.Multiline);
            return to_modify;
        }
        else
        {
            return string.Empty;
        }
    }

    private static XElement CreateBody(XDocument xliffDocument, XNamespace defaultNs, bool ignoreChildren)
    {
        var body = new XElement(ns + "body");
        
        foreach (var transUnit in xliffDocument.Descendants(defaultNs + "trans-unit"))
        {
            var newTransUnitElement = new XElement(ns + "trans-unit",
                new XAttribute("id", transUnit.Attribute("id")?.Value ?? "unknown")
            );

            var sourceElement = new XElement(ns + "source", Regex.Replace(transUnit.Element(defaultNs + "source").ToString(), @"</?source(.*?)>", ""));             
            if (sourceElement != null) newTransUnitElement.Add(sourceElement);

            var targetElement = new XElement(ns + "target", Regex.Replace(transUnit.Element(defaultNs + "target").ToString(), @"</?target(.*?)>", ""));
            if (targetElement != null) newTransUnitElement.Add(targetElement);

            body.Add(newTransUnitElement);
        }

        return body;
    }

    private static XElement ProcessTransUnitElement(XElement element, bool ignoreChildren)
    {
        // Create a new element that matches the name of the original, ensuring it's in the correct namespace
        var newElement = new XElement(ns + element.Name.LocalName);

        // Check if the element contains nested elements (e.g., <ph> tags)
        foreach (var node in element.Nodes())
        {
            if (node is XElement childElement && !ignoreChildren)
            {
                // Recursively process nested elements to support deep structures
                var processedChild = ProcessTransUnitElement(childElement, ignoreChildren);
                newElement.Add(processedChild);
            }
            else if (node is XText textNode)
            {
                newElement.Add(ProcessTextNodeValue(element.Name.LocalName, textNode.Value));
            }
        }

        return newElement;
    }
    
    private static string ProcessTextNodeValue(string elementName, string textValue)
    {
        if (elementName == "source" || elementName == "target")
        {
            return textValue.TrimStart();
        }

        return textValue;
    }
}
