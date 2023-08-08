using App.Services.Certificate;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Helpers;

public static class ServicesFactory
{
    public static ICertificateService CreateCertificateService()
    {
        var serviceProvider = new ServiceProviderBuilder()
            .WithDefaultSettings()
            .WithCertificateManager()
            .Build();

        return serviceProvider.GetRequiredService<ICertificateService>();
    }
}