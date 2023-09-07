using System.Xml.Serialization;
using Apps.Memoq.Callbacks.Models.Payload.DocumentDelivery;

namespace Apps.Memoq.Callbacks.Models.Payload.Base;

[XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class Envelope
{
    public DocumentDeliveryBody Body { get; set; }
}