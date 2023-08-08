using App.Commands;
using App.Configuration;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Tests.Commands.Fakes;
using Tests.Helpers;

namespace Tests.Commands;

public class EccCommandTests
{
    [Theory]
    [InlineData("Ecc1.pfx", "Pass1", 256, 1)]
    [InlineData("Ecc2.pfx", "Pass2", 384, 2)]
    [InlineData("Ecc3.pfx", "Pass3", 521, 3)]
    public void Should_Generate_EccCertificate(string certificateFile, string certificatePassword, int keySize, int years)
    {
        // arrange
        var app = new CommandLineApplication();
        var certificateService = ServicesFactory.CreateCertificateService();
        var consoleService = new FakeConsoleService();
        var command = new EccCommand(certificateService, consoleService)
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
    [InlineData("", "Pass", 256, 1)]
    [InlineData("Ecc.pfx", "", 384, 2)]
    [InlineData("Ecc.pfx", "Pass", 100, 3)]
    [InlineData("Ecc.pfx", "Pass", 521, 0)]
    [InlineData("Ecc.pfx", "Pass", 521, -1)]
    [InlineData("Ecc.pfx", "Pass", 521, 1000)]
    public void Should_Not_Generate_EccCertificate(string certificateFile, string certificatePassword, int keySize, int years)
    {
        // arrange
        var app = new CommandLineApplication();
        var certificateService = ServicesFactory.CreateCertificateService();
        var consoleService = new FakeConsoleService();
        var command = new EccCommand(certificateService, consoleService)
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