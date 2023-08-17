namespace Apps.Memoq.Models.Files.Responses
{
    public class GetAnalysesForAllDocumentsResponse
    {
        public IEnumerable<GetAnalysisResponse> Analyses { get; set; }
    }
}
