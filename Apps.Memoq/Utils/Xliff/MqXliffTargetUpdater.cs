using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Apps.Memoq.Utils.Xliff;

public static class MqXliffTargetUpdater
{
    private static readonly XNamespace Xliff12Ns = "urn:oasis:names:tc:xliff:document:1.2";
    private static readonly XNamespace Xliff21Ns = "urn:oasis:names:tc:xliff:document:2.0";
    private static readonly XNamespace MqNs = "MQXliff";

    private static readonly HashSet<string> ConfirmedStatuses = new()
    {
        "ManuallyConfirmed", "Reviewer1Confirmed", "Proofread"
    };

    public static Stream UpdateTargets(Stream mqXliffFile, Stream xliffFile, bool updateLocked = true,
        bool updateConfirmed = true)
    {
        var xliffDoc = XDocument.Load(xliffFile);
        var mqXliffDoc = XDocument.Load(mqXliffFile);

        var xliffUnits = xliffDoc.Descendants(Xliff21Ns + "unit");

        foreach (var mqTransUnit in mqXliffDoc.Descendants(Xliff12Ns + "trans-unit"))
        {
            var id = mqTransUnit.Attribute("id")?.Value;
            var mqTarget = mqTransUnit.Elements(Xliff12Ns + "target").FirstOrDefault();

            var xliffUnit = xliffUnits.FirstOrDefault(x => x.Attribute("id")?.Value == id);
            var xliffTargetNodes = xliffUnit?.Elements(Xliff21Ns + "segment").Elements(Xliff21Ns + "target").Nodes()
                .Select(WithoutNamespaceDeclarations)
                .ToList();

            if (mqTarget != null && xliffTargetNodes != null &&
                !mqTarget.Nodes().Select(WithoutNamespaceDeclarations)
                    .SequenceEqual(xliffTargetNodes, new XNodeEqualityComparer()))
            {
                if (!updateLocked && mqTransUnit.Attribute(MqNs + "locked")?.Value == "locked")
                {
                    continue;
                }

                if (!updateConfirmed && ConfirmedStatuses.Contains(mqTransUnit.Attribute(MqNs + "status")?.Value ?? string.Empty))
                {
                    continue;
                }

                mqTarget.Nodes().Remove();
                mqTarget.Add(xliffTargetNodes);
                mqTransUnit.SetAttributeValue(MqNs + "status", "Edited");
            }
        }

        var updatedString = Regex.Replace(mqXliffDoc.ToString(),
                "(<(source(.*?)|\\/bpt|\\/ph|target(.*?))>)\\r?\\n\\s+(?!\\s?(<target|<\\/trans-unit>))", "${1}");
        updatedString = Regex.Replace(updatedString,"&(?!(\\w+|#\\d+);)", "&amp;");
        var updatedMqXliffStream = new MemoryStream(Encoding.UTF8.GetBytes(updatedString));
        updatedMqXliffStream.Position = 0;
        return updatedMqXliffStream;
    }

    private static XNode WithoutNamespaceDeclarations(XNode node)
    {
        if (node is not XElement element)
        {
            return node;
        }

        var copy = new XElement(element);
        foreach (var descendant in copy.DescendantsAndSelf())
        {
            descendant.Attributes().Where(attribute => attribute.IsNamespaceDeclaration).Remove();
        }

        return copy;
    }
}
