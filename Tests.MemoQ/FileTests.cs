using Apps.Memoq.Actions;
using Apps.Memoq.Models.Files.Requests;
using Apps.MemoQ.Models.Files.Requests;
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
        private FileActions Actions => new(InvocationContext, FileManager);
        
        [TestMethod]
        public async Task Report_returns_report_values()
        {
            var result = await Actions.GetEditDistanceReport(
                new ProjectRequest { ProjectGuid = "10bd767d-3ce2-ef11-875f-a8a15994f72e" },
                "a25715b6-c004-f011-875f-a8a15994f72e");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task DownloadFileByGuid_IsSuccess()
        {
            var response = await Actions.DownloadFileByGuid(new DownloadFileRequest
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

            var result = await Actions.GetPostTranslationAnalysisReport(
                new ProjectRequest { ProjectGuid = PostTranslationProjectGuid },
                reportId!);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Files.Any());
        }

        [TestMethod]
        public async Task ExportProjectAnalysis_IsSuccess()
        {
            // Arrange
            var projectId = new ProjectRequest { ProjectGuid = "2e25affd-731a-f111-875f-a8a15994f72e" };
            var input = new ExportProjectAnalysisRequest
            {
                
            };
            
            // Act
            var result = await Actions.ExportProjectAnalysis(projectId, input);

            // Assert
            Console.WriteLine(result.ExportedAnalysis.Name);
        }

        [TestMethod]
        public async Task GetAnalysesForAllDocuments_IsSuccess()
        {
            // Arrange
            var input = new GetAnalysisForProjectRequest
            {
                ProjectGuid = "1399ad04-b515-f111-875f-a8a15994f72e",
                Format = "CSV_MemoQ"
            };

            // Act
            var result = await Actions.GetAnalysesForAllDocuments(input);

            // Assert
            foreach (var analysis in result.Analyses)
                Console.WriteLine(analysis.ResultData.Name);
        }
    }
}
