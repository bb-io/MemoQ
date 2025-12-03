namespace Apps.MemoQ.Events.Polling.Models.Memory
{
    public class AllFilesDeliveredMemory
    {
        public bool IsCompleted { get; set; }
        public DateTime LastCheckDate { get; set; }
        public int DeliveredCount { get; set; }
        public int TotalCount { get; set; }
    }
}
