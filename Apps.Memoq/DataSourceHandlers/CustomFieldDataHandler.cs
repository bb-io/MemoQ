using Apps.Memoq.Models.Files.Requests;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using MQS.ServerProject;
using Apps.Memoq.Actions;

namespace Apps.MemoQ.DataSourceHandlers
{
    public class CustomFieldDataHandler : BaseInvocable, IDataSourceHandler
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
            InvocationContext.AuthenticationCredentialsProviders;

        private readonly string _projectGuid;
        private readonly InvocationContext _context;

        public CustomFieldDataHandler(InvocationContext invocationContext, [ActionParameter] GetDocumentRequest request) :
            base(invocationContext)
        {
            _projectGuid = request.ProjectGuid;
            _context = invocationContext;
        }

        public Dictionary<string, string> GetData(DataSourceContext context)
        {
            if (string.IsNullOrEmpty(_projectGuid))
            {
                throw new InvalidOperationException("You should input a project guid first");
            }

            var actions = new ServerProjectActions(_context);
            var response = actions.GetCustomFields(new Memoq.Models.ServerProjects.Requests.ProjectRequest 
            {ProjectGuid = _projectGuid });

            return response.CustomFields
                .Where(x => context.SearchString is null ||
                            x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x => x.Name)
                .Take(20)
                .ToDictionary(x => x.Name, x => x.Name);
        }
    }
}
