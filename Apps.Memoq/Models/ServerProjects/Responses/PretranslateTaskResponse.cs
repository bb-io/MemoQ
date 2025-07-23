using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Models.ServerProjects.Responses
{
    public class PretranslateTaskResponse
    {
        [Display("Task ID")]
        public string TaskId { get; set; }

        [Display("Task Status")]
        public string TaskStatus { get; set; }

        [Display("Progress Percentage")]
        public short ProgressPercentage { get; set; }
    }
}
