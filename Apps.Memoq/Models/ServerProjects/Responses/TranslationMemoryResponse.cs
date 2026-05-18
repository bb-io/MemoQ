using Apps.Memoq.Models.Dto;
using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Models.ServerProjects.Responses;
public class TranslationMemoryResponse
{
    [Display("Translation memory IDs")]
    public List<string> TranslationMemoryIds { get; set; } = new List<string>();

    [Display("Primary TM ID")]
    public string PrimaryTmId { get; set; }

    [Display("Master TM ID")]
    public string MasterTmId { get; set; }

    [Display("Translation memories")]
    public List<TmDto> TranslationMemories { get; set; } = new List<TmDto>();
}