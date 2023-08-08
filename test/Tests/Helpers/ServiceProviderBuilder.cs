using App.Configuration;
using App.Services.Certificate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Tests.Helpers;

public class ServiceProviderBuilder
{
    private readonly IServiceCollection _services = new ServiceCollection();
    
    public ServiceProviderBuilder WithDefaultSettings()
    {
        var settings = new Settings
        {
            KeyUsageFlags = Array.Empty<string>()
        };

        var options = Options.Create(settings);
        _services.AddTransient(_ => options);
        return this;
    }

    public ServiceProviderBuilder WithCertificateManager()
    {
        _services.AddCertificateManager();
        _services.AddTransient<ICertificateHelper, CertificateHelper>();
        _services.AddTransient<ICertificateService, CertificateService>();
        return this;
    }

    public ServiceProvider Build()
    {
        return _services.BuildServiceProvider();
    }
}