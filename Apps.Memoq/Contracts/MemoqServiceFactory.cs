﻿using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;

namespace Apps.Memoq.Contracts;

public sealed class MemoqServiceFactory<T> : IDisposable
{
    private readonly ChannelFactory<T> _channelFactory;

    public MemoqServiceFactory(string serviceUrl, string apiKey)
    {
        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        var header = AddressHeader.CreateAddressHeader("ApiKey", "", apiKey);
        var address = new EndpointAddress(new Uri(serviceUrl), header);
        _channelFactory = new ChannelFactory<T>(binding, address);
        _channelFactory.Credentials.ServiceCertificate.SslCertificateAuthentication = 
        new X509ServiceCertificateAuthentication()
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