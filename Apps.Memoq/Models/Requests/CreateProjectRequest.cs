namespace Apps.Memoq.Models.Requests;

public class CreateProjectRequest
{
    public string SourseLangCode { get; set; }
    
    public IEnumerable<string> TargetLangCodes { get; set; }

    public string ProjectName { get; set; }
}