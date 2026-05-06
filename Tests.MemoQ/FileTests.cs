using Apps.Memoq.Actions;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.MemoQ.Models.ServerProjects.Requests;
using Newtonsoft.Json;
using Tests.MemoQ.Base;

namespace Tests.MemoQ
{
    [TestClass]
    public class FileTests : TestBase
    {
        private const string PostTranslationProjectGuid = "ec423da3-ecef-ed11-85f6-d05099f919f4";

        [TestMethod]
        public async Task Report_returns_report_values()
        {
            var handler = new FileActions(InvocationContext, FileManager);

            var result = await handler.GetEditDistanceReport(
                new ProjectRequest { ProjectGuid = "10bd767d-3ce2-ef11-875f-a8a15994f72e" },
                "a25715b6-c004-f011-875f-a8a15994f72e");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task DownloadFileByGuid_IsSuccess()
        {
            var action = new FileActions(InvocationContext, FileManager);

            var response = await action.DownloadFileByGuid(new DownloadFileRequest
            {
                ProjectGuid = "10bd767d-3ce2-ef11-875f-a8a15994f72e",
                DocumentGuid = "0d928a55-8070-48db-9e3e-00723fe85cfd"
            });

            Console.WriteLine(JsonConvert.SerializeObject(response));
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task ExportPostTranslationAnalysisReport_IsSuccess()
        {
            var serverProjectActions = new ServerProjectActions(InvocationContext, FileManager);
            await serverProjectActions.GeneratePostTranslationAnalysisReport(
                new ProjectRequest { ProjectGuid = "3bd942d0-93ed-f011-875f-a8a15994f72e" },
                new PostTranslationAnalysisRequest
                {
                    StoreReportInProject = true,
                });

            var reports = await serverProjectActions.SearchPostTranslationAnalysisReports(
                new ProjectRequest { ProjectGuid = "3bd942d0-93ed-f011-875f-a8a15994f72e" });
            var reportId = reports.Reports.FirstOrDefault()?.ReportId;

            Assert.IsFalse(string.IsNullOrWhiteSpace(reportId), "No post translation analysis report was found for export.");

            var fileActions = new FileActions(InvocationContext, FileManager);
            var result = await fileActions.GetPostTranslationAnalysisReport(
                new ProjectRequest { ProjectGuid = PostTranslationProjectGuid },
                reportId!);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Files.Any());
        }
    }
}
