using Blackbird.Filters.Analysis.Models;

namespace Apps.MemoQ.Utils.Analysis;

public static class AnalysisHelper
{
    public static readonly Dictionary<string, AnalysisType> AnalysisMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "101% Words", AnalysisType.ExactMatch },
        { "100% Words", AnalysisType.ExactMatch },
        { "95% - 99% Words", AnalysisType.Match9599 },
        { "85% - 94% Words", AnalysisType.Match8594 },
        { "75% - 84% Words", AnalysisType.Match7584 },
        { "50% - 74% Words", AnalysisType.Match5074 },
        { "No match Words", AnalysisType.NoMatch },
        { "Repetitions Words", AnalysisType.Repetition }
    };
}