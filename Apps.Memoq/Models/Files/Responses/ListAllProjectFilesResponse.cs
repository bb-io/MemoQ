using Apps.Memoq.Models.Dto;
using Apps.MemoQ.Models.Dto;

namespace Apps.Memoq.Models.Files.Responses;

public class ListAllProjectFilesResponse
{
    public FileInfoDto[] Files { get; set; }
}