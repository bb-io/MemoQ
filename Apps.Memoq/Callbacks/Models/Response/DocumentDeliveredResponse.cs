using Apps.Memoq.Callbacks.Models.Payload.DocumentDelivery;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Callbacks.Models.Response;

public class DocumentDeliveredResponse
{
    [Display("Project GUID")]
    public string ProjectGuid { get; set; }

    [Display("Project name")]
    public string ProjectName { get; set; }

    [Display("Document GUID")]
    public string DocumentGuid { get; set; }

    [Display("Document name")]
    public string DocumentName { get; set; }

    [Display("External document ID")]
    public string ExternalDocumentId { get; set; }

    [Display("New assigned user ID")]
    public string NewAssignedUserId { get; set; }

    [Display("New assigned user name")]
    public string NewAssignedUserName { get; set; }

    [Display("New workflow status")]
    public string NewWorkflowStatus { get; set; }

    [Display("Previous workflow status")]
    public string PreviousWorkflowStatus { get; set; }

    [Display("Target language code")]
    public string TargetLanguageCode { get; set; }
    
    public DocumentDeliveredResponse(DocumentDeliveryData payload)
    {
        ProjectGuid = payload.ProjectInfo.ProjectGuid;
        ProjectName = payload.ProjectInfo.ProjectName;
        DocumentGuid = payload.DeliveredItems.DeliveryItem.DocumentGuid;
        DocumentName = payload.DeliveredItems.DeliveryItem.DocumentName;
        ExternalDocumentId = payload.DeliveredItems.DeliveryItem.ExternalDocumentId;
        NewAssignedUserId = payload.DeliveredItems.DeliveryItem.NewAssignedUserId;
        NewAssignedUserName = payload.DeliveredItems.DeliveryItem.NewAssignedUserName;
        NewWorkflowStatus = payload.DeliveredItems.DeliveryItem.NewWorkflowStatus;
        PreviousWorkflowStatus = payload.DeliveredItems.DeliveryItem.PreviousWorkflowStatus;
        TargetLanguageCode = payload.DeliveredItems.DeliveryItem.TargetLanguageCode;
    }
}