using System.Xml.Serialization;

namespace Apps.Memoq.Callbacks.Models.Payload.DocumentDelivery;

public class DocumentDeliveryBody
{
    [XmlElement(Namespace = "http://kilgray.com/memoq/v1")]
    public DocumentDeliveryData DocumentDelivery { get; set; }
}