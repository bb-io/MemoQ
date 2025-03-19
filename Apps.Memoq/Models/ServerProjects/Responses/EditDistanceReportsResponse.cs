using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Models.ServerProjects.Responses
{
    public class EditDistanceReportsResponse
    {
        public IEnumerable<EditDistanceReportResponse> Reports { get; set; }

        public EditDistanceReportsResponse(MQS.ServerProject.EditDistanceReportInfo[] reports)
        {
            Reports = reports.Select(r => new EditDistanceReportResponse(r)).ToList();
        }
    }

    public class EditDistanceReportResponse
    {
        [Display("Created at")]
        public DateTime Created { get; set; }

        [Display("Created by")]
        public string CreatedBy { get; set; }

        [Display("Distance measurement")]
        public MQS.ServerProject.DistanceMeasurementMode DistanceMeasurement { get; set; }

        [Display("Languages")]
        public IEnumerable<string> Languages { get; set; }

        [Display("Note")]
        public string Note { get; set; }

        [Display("Report ID")]
        public Guid ReportId { get; set; }
        public EditDistanceReportResponse(MQS.ServerProject.EditDistanceReportInfo info)
        {
            Created = info.Created;
            CreatedBy = info.CreatedBy;
            DistanceMeasurement = info.DistanceMeasurement;
            Languages = info.Languages;
            Note = info.Note;
            ReportId = info.ReportId;
        }
    }
}
