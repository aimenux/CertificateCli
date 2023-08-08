using App.Commands;
using App.Configuration;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Tests.Commands.Fakes;
using Tests.Helpers;

namespace Tests.Commands;

public class RsaCommandTests
{
    [Theory]
    [InlineData("Rsa1.pfx", "Pass1", 2048, 1)]
    [InlineData("Rsa2.pfx", "Pass2", 3072, 2)]
    [InlineData("Rsa3.pfx", "Pass3", 7680, 3)]
    public void Should_Generate_RsaCertificate(string certificateFile, string certificatePassword, int keySize, int years)
    {
        // arrange
        var app = new CommandLineApplication();
        var certificateService = ServicesFactory.CreateCertificateService();
        var consoleService = new FakeConsoleService();
        var command = new RsaCommand(certificateService, consoleService)
        {
            CertificateFile = certificateFile,
            CertificatePassword = certificatePassword,
            ValidityInYears = years,
            KeySize = keySize
        };
        
        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ok);
    }
    
    [Theory]
    [InlineData("", "Pass", 2048, 1)]
    [InlineData("Rsa.pfx", "", 3072, 2)]
    [InlineData("Rsa.pfx", "Pass", 100, 3)]
    [InlineData("Rsa.pfx", "Pass", 7680, 0)]
    [InlineData("Rsa.pfx", "Pass", 7680, -1)]
    [InlineData("Rsa.pfx", "Pass", 7680, 1000)]
    public void Should_Not_Generate_RsaCertificate(string certificateFile, string certificatePassword, int keySize, int years)
    {
        // arrange
        var app = new CommandLineApplication();
        var certificateService = ServicesFactory.CreateCertificateService();
        var consoleService = new FakeConsoleService();
        var command = new RsaCommand(certificateService, consoleService)
        {
            CertificateFile = certificateFile,
            CertificatePassword = certificatePassword,
            ValidityInYears = years,
            KeySize = keySize
        };
        
        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ko);
    }
}