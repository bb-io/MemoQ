using System.Security.Cryptography;
using System.Text;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;

namespace Apps.Memoq.Extensions;

public static class BridgeExtensions
{
    public static string GetInstanceUrlHash(this IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var hash = new StringBuilder();
        var instanceUrl = creds.Get("url").Value;

        var crypto = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(instanceUrl));
        foreach (byte theByte in crypto)
            hash.Append(theByte.ToString("x2"));

        return hash.ToString();
    }
}