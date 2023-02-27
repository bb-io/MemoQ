namespace Apps.Memoq.Models.Responses;

public class TestOutputResponse
{
    public string ProjectGuid { get; set; }

    public string ProjectDeadline { get; set; }

    public string SourceLanguage { get; set; }
    
    public IEnumerable<string> TargetLanguages { get; set; }

    public CreateProjectResponse ProjectResponse { get; set; }

    public IEnumerable<CreateProjectResponse> CreateProjectResponses { get; set; }
}