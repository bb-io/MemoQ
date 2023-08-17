namespace Apps.Memoq.Constants;

public static class EnumValues
{
    public static readonly string[] TMEngineType = { "OldTMEngine", "NGTMEngine" };
    public static readonly string[] TMOptimizationPreference = { "Recall", "Mixed", "Fast" };
    public static readonly string[] Format = { "Html", "CSV_WithTable", "CSV_Trados", "CSV_MemoQ" };
    public static readonly string[] Algorithm = { "MemoQ", "Trados" };
    public static readonly string[] XTranslateScenario = { "NewRevision", "MidProjectUpdate" };

    public static readonly string[] ExpectedFinalState =
        { "SameAsPrevious", "Pretranslated", "Confirmed", "Proofread", "Reviewer1Confirmed" };

    public static readonly string[] SourceFilter =
        { "ProofreadOnly", "ProofreadOrConfirmed", "AllTarget" };
}