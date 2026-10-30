using System.Text;
using System.Xml.Linq;
using Apps.Memoq.Utils.Xliff;

namespace Tests.MemoQ;

[TestClass]
public class MqXliffTargetUpdaterTests
{
    private static readonly XNamespace Xliff12Ns = "urn:oasis:names:tc:xliff:document:1.2";
    private static readonly XNamespace MqNs = "MQXliff";

    private const string MqXliff = """
        <?xml version="1.0" encoding="utf-8"?>
        <xliff version="1.2" xmlns="urn:oasis:names:tc:xliff:document:1.2" xmlns:mq="MQXliff">
          <file original="story.docx" source-language="eng" target-language="spa" datatype="x-undefined">
            <body>
              <trans-unit id="1" mq:status="PreTranslated" mq:percent="101">
                <source xml:space="preserve"><bpt id="1" ctype="bold">{}</bpt>Bold<ept id="1">{}</ept> and <bpt id="2" ctype="italic">{}</bpt>italic<ept id="2">{}</ept> text</source>
                <target xml:space="preserve"><bpt id="1" ctype="bold">{}</bpt>Negrita<ept id="1">{}</ept> y <bpt id="2" ctype="italic">{}</bpt>cursiva<ept id="2">{}</ept> texto</target>
              </trans-unit>
              <trans-unit id="2" mq:status="PreTranslated" mq:percent="100">
                <source xml:space="preserve">Plain text</source>
                <target xml:space="preserve">Texto plano</target>
              </trans-unit>
              <trans-unit id="3" mq:status="PreTranslated" mq:percent="100">
                <source xml:space="preserve">Line with <ph id="1">{}</ph> placeholder</source>
                <target xml:space="preserve">Línea con <ph id="1">{}</ph> marcador</target>
              </trans-unit>
            </body>
          </file>
        </xliff>
        """;

    [TestMethod]
    public void UpdateTargets_UnchangedMqXliffWithInlineFormatting_KeepsSegmentStatuses()
    {
        var updatedDocument = UpdateMqXliffWith(MqXliff);

        CollectionAssert.AreEqual(
            new[] { "PreTranslated", "PreTranslated", "PreTranslated" },
            GetStatuses(updatedDocument));
    }

    [TestMethod]
    public void UpdateTargets_ChangedFormattedTarget_MarksOnlyThatSegmentEdited()
    {
        var changedMqXliff = MqXliff.Replace("cursiva<ept", "itálica<ept");

        var updatedDocument = UpdateMqXliffWith(changedMqXliff);

        CollectionAssert.AreEqual(
            new[] { "Edited", "PreTranslated", "PreTranslated" },
            GetStatuses(updatedDocument));
        Assert.IsTrue(
            XNode.DeepEquals(GetTarget(XDocument.Parse(changedMqXliff), "1"), GetTarget(updatedDocument, "1")),
            $"Unexpected edited target: {GetTarget(updatedDocument, "1")}");
    }

    [TestMethod]
    public void UpdateTargets_UnchangedMqXliffWithInlineFormatting_KeepsTargetsIntact()
    {
        var originalDocument = XDocument.Parse(MqXliff);

        var updatedDocument = UpdateMqXliffWith(MqXliff);

        foreach (var id in new[] { "1", "2", "3" })
        {
            Assert.IsTrue(XNode.DeepEquals(GetTarget(originalDocument, id), GetTarget(updatedDocument, id)),
                $"Target of unit {id} changed: {GetTarget(updatedDocument, id)}");
        }
    }

    private static XDocument UpdateMqXliffWith(string uploadedMqXliff)
    {
        var uploadedAsXliff21 = new MemoryStream();
        XDocument.Parse(uploadedMqXliff).ConvertToTwoPointOne().Save(uploadedAsXliff21);
        uploadedAsXliff21.Position = 0;

        var downloadedMqXliff = new MemoryStream(Encoding.UTF8.GetBytes(MqXliff));

        var updated = MqXliffTargetUpdater.UpdateTargets(downloadedMqXliff, uploadedAsXliff21);
        return XDocument.Load(updated, LoadOptions.PreserveWhitespace);
    }

    private static string?[] GetStatuses(XDocument document) =>
        document.Descendants(Xliff12Ns + "trans-unit")
            .Select(unit => unit.Attribute(MqNs + "status")?.Value)
            .ToArray();

    private static XElement GetTarget(XDocument document, string unitId) =>
        document.Descendants(Xliff12Ns + "trans-unit")
            .Single(unit => unit.Attribute("id")?.Value == unitId)
            .Element(Xliff12Ns + "target")!;
}
