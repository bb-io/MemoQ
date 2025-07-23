using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Events.Polling.Models
{
    public class TaskStatusMemory
    {
        public string Status { get; set; }
        public DateTime LastCheckDate { get; set; }
    }
    public class TaskStatusResponse
    {
        [Display("Task ID")]
        public string TaskId { get; set; }

        [Display("Task status")]
        public string TaskStatus { get; set; }

        [Display("Progress percentage")]
        public short ProgressPercentage { get; set; }
    }
}
