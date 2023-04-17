namespace Apps.Memoq.Models.Requests;

public class CreateProjectRequest
{
    public string SourceLangCode { get; set; }
    
    public IEnumerable<string> TargetLangCodes { get; set; }

    public string ProjectName { get; set; }
}