using System.Net;

namespace Apps.Memoq.Models.Responses;

public class GenerateFileResponse
{
    public HttpStatusCode StatusCode { get; set; }

    public string[] Files { get; set; }

    public int Total { get; set; }
}