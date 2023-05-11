namespace Apps.Memoq.Models.Files.Requests;

public class UploadDocumentToProjectRequest
{
    public byte[] File { get; set; }
    public string FileName { get; set; }

    public string ProjectGuid { get; set; }
}