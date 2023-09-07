namespace Apps.Memoq.Callbacks.Models.Payload.DocumentDelivery;

public class DeliveryItem
{
    public string DocumentGuid { get; set; }

    public string DocumentName { get; set; }
    
    public string ExternalDocumentId { get; set; }
    
    public string NewAssignedUserId { get; set; }
    
    public string NewAssignedUserName { get; set; }

    public string NewWorkflowStatus { get; set; }
    
    public string PreviousWorkflowStatus { get; set; }
    
    public string TargetLanguageCode { get; set; }
}