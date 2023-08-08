using App.Commands;
using App.Configuration;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Tests.Commands.Fakes;
using Tests.Helpers;

namespace Tests.Commands;

public class JwkCommandTests
{
    [Theory]
    [InlineData("Files/RSA.pfx", "4-tests", "1", Settings.DefaultKeyUse)]
    [InlineData("Files/ECC.pfx", "4-tests", "1", Settings.DefaultKeyUse)]
    public void Should_Generate_Jwk(string certificateFile, string certificatePassword, string keyId, string keyUse)
    {
        // arrange
        var app = new CommandLineApplication();
        var certificateService = ServicesFactory.CreateCertificateService();
        var consoleService = new FakeConsoleService();
        var command = new JwkCommand(certificateService, consoleService)
        {
            CertificateFile = certificateFile,
            CertificatePassword = certificatePassword,
            KeyId = keyId,
            KeyUse = keyUse
        };
        
        // act
        var result = command.OnExecute(app);

        // assert
        File.Exists(certificateFile).Should().BeTrue($"{certificateFile} does not exist.");
        result.Should().Be(Settings.ExitCode.Ok);
    }
    
    [Theory]
    [InlineData("Files/FOO.pfx", "4-tests", "1", Settings.DefaultKeyUse)]
    [InlineData("Files/RSA.pfx", "4-tests", "", Settings.DefaultKeyUse)]
    [InlineData("Files/ECC.pfx", "4-tests", "1", "")]
    public void Should_Not_Generate_Jwk(string certificateFile, string certificatePassword, string keyId, string keyUse)
    {
        // arrange
        var app = new CommandLineApplication();
        var certificateService = ServicesFactory.CreateCertificateService();
        var consoleService = new FakeConsoleService();
        var command = new JwkCommand(certificateService, consoleService)
        {
            CertificateFile = certificateFile,
            CertificatePassword = certificatePassword,
            KeyId = keyId,
            KeyUse = keyUse
        };
        
        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ko);
    }
}