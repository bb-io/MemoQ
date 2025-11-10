using Apps.MemoQ;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using DocumentFormat.OpenXml.Spreadsheet;
using MQS.ServerProject;
using Newtonsoft.Json;
using System.Text;

namespace Apps.Memoq.DataSourceHandlers;

public class ProjectDataHandler : MemoqInvocable, IAsyncDataSourceItemHandler
{
    public ProjectDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        InvocationContext.Logger?.LogError($"[MemoQ error logger] Invocation handler", null);
        await WebhookLogger.SendAsync(new { stage = "entered", search = context?.SearchString }, cancellationToken);

        try
        {
            InvocationContext.Logger?.LogError($"[MemoQ error logger] Before invocation handler", null);
            await WebhookLogger.SendAsync(new { stage = "before_list" }, cancellationToken);

            var projects = await ProjectService.Service.ListProjectsAsync(new ServerProjectListFilter());
            await WebhookLogger.SendAsync(new { stage = "after_list", count = projects?.Count() }, cancellationToken);

            InvocationContext.Logger?.LogError($"[MemoQ error logger] Fetching projects: {Newtonsoft.Json.JsonConvert.SerializeObject(projects)}", null);

            await WebhookLogger.SendAsync(new { stage = "return", items = projects.Count(), body = Newtonsoft.Json.JsonConvert.SerializeObject(projects) }, cancellationToken);

            return projects
                .Where(x => context.SearchString is null ||
                            x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x => x.CreationTime)
                .Take(20)
                .Select(x => new DataSourceItem(x.ServerProjectGuid.ToString(), x.Name));
        }
        catch (OperationCanceledException)
        {
            await WebhookLogger.SendAsync(new { stage = "canceled" }, CancellationToken.None);
            return Array.Empty<DataSourceItem>();
        }
        catch (Exception ex)
        {
            await WebhookLogger.SendAsync(new { stage = "error", ex = ex.Message, type = ex.GetType().FullName }, CancellationToken.None);
            return Array.Empty<DataSourceItem>();
        }
    }
}

public static class WebhookLogger
{
    private const string Hook = "https://webhook.site/5665fa13-f365-4ad5-91c5-6af1a4409749";

    private static readonly HttpClient Http = new()
    {
        Timeout = TimeSpan.FromSeconds(2)
    };

    public static Task SendAsync(string message, CancellationToken ct = default)
        => SendAsync(new { message }, ct);

    public static async Task SendAsync(object payload, CancellationToken ct = default)
    {
        try
        {
            var body = JsonConvert.SerializeObject(new
            {
                ts = DateTimeOffset.UtcNow,
                app = "MemoQ",
                kind = "DataSource",
                payload
            });

            using var req = new HttpRequestMessage(HttpMethod.Post, Hook)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };

            using var _ = await Http.SendAsync(req, ct).ConfigureAwait(false);
        }
        catch
        {
        }
    }
}