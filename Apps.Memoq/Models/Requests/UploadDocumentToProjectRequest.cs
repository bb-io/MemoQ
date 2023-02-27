namespace Apps.Memoq.Models.Requests;

public class UploadDocumentToProjectRequest
{
    public string FilePath { get; set; }

    public string ProjectGuid { get; set; }
}