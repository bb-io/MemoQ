using Blackbird.Applications.Sdk.Common.Authentication;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;

namespace Apps.Memoq.Contracts;

public sealed class MemoqServiceFactory<T> : IDisposable
{
    private readonly ChannelFactory<T> _channelFactory;

    public MemoqServiceFactory(string serviceUrl,
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var apiKey = authenticationCredentialsProviders.Get("apiKey").Value;
        var url = authenticationCredentialsProviders.Get("url").Value;

        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport)
        {
            MaxReceivedMessageSize = int.MaxValue
        };
        var header = AddressHeader.CreateAddressHeader("ApiKey", "", apiKey);
        var address = new EndpointAddress(new Uri($"{url}{serviceUrl}"), header);
        _channelFactory = new ChannelFactory<T>(binding, address);
        _channelFactory.Credentials.ServiceCertificate.SslCertificateAuthentication =
            new()
            {
                CertificateValidationMode = X509CertificateValidationMode.None,
                RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck
            };
        Service = _channelFactory.CreateChannel();
    }

    public T Service { get; }

    public void Dispose()
    {
        try
        {
            _channelFactory.Close();
        }
        catch
        {
            // ignored
        }
    }
}