using System.Xml.Serialization;

namespace Apps.Memoq.Callbacks.Models.Payload.DocumentDelivery;

public class DocumentDeliveryData
{
    [XmlElement(ElementName = "projectInfo")]
    public ProjectInfo ProjectInfo { get; set; }

    [XmlElement(ElementName = "deliveredItems")]
    public DeliveredItems DeliveredItems { get; set; }
}