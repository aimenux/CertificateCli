using App.Configuration;
using App.Services.Certificate;
using FluentAssertions;
using Tests.Helpers;

namespace Tests.Services;

public class CertificateServiceTests
{
    [Theory]
    [InlineData(2048)]
    [InlineData(3072)]
    [InlineData(7680)]
    public void Should_Generate_RsaCertificate(int keySize)
    {
        // arrange
        var uniqueId = Guid.NewGuid().ToString("N");
        var certificateService = ServicesFactory.CreateCertificateService();
        var pfxParameters = new PfxParameters
        {
            CertificateFile = $"RSA-{uniqueId}.pfx",
            CertificatePassword = $"Pass-{uniqueId}",
            DnsName = $"DNS-{uniqueId}",
            KeySize = keySize,
            OutputDirectory = "."
        };

        // act
        var generateRsa = () => certificateService.GenerateRsa(pfxParameters);

        // assert
        generateRsa.Should().NotThrow<Exception>();
        pfxParameters.CertificateFullPath.Should().Match(x => File.Exists(x));
    }
    
    [Theory]
    [InlineData(256)]
    [InlineData(384)]
    [InlineData(521)]
    public void Should_Generate_EccCertificate(int keySize)
    {
        // arrange
        var uniqueId = Guid.NewGuid().ToString("N");
        var certificateService = ServicesFactory.CreateCertificateService();
        var pfxParameters = new PfxParameters
        {
            CertificateFile = $"ECC-{uniqueId}.pfx",
            CertificatePassword = $"Pass-{uniqueId}",
            DnsName = $"DNS-{uniqueId}",
            KeySize = keySize,
            OutputDirectory = "."
        };

        // act
        var generateEcc = () => certificateService.GenerateEcc(pfxParameters);

        // assert
        generateEcc.Should().NotThrow<Exception>();
        pfxParameters.CertificateFullPath.Should().Match(x => File.Exists(x));
    }
    
    [Theory]
    [InlineData("Files/RSA.pfx", "4-tests", "1", Settings.DefaultKeyUse)]
    [InlineData("Files/ECC.pfx", "4-tests", "1", Settings.DefaultKeyUse)]
    public void Should_Generate_Jwk(string certificateFile, string certificatePassword, string keyId, string keyUse)
    {
        // arrange
        var certificateService = ServicesFactory.CreateCertificateService();
        var jwkParameters = new JwkParameters
        {
            CertificateFile = certificateFile,
            CertificatePassword = certificatePassword,
            KeyId = keyId,
            KeyUse = keyUse
        };

        // act
        var jwk = certificateService.GenerateJwk(jwkParameters);

        // assert
        jwk.Should().NotBeNullOrWhiteSpace();
    }
}