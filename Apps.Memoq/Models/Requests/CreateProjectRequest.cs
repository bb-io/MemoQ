namespace Apps.Memoq.Models.Requests;

public class CreateProjectRequest
{
    public string SourceLangCode { get; set; }
    
    public string TargetLangCodes { get; set; }

    public string ProjectName { get; set; }
}