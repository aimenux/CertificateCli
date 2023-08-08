using App.Commands;
using App.Configuration;
using App.Services.Certificate;
using App.Services.Console;
using App.Validators;
using FluentAssertions;
using NSubstitute;

namespace Tests.Validators;

public class JwkCommandValidatorTests
{
    [Theory]
    [InlineData("Files/RSA.pfx", "4-tests", "1", Settings.DefaultKeyUse)]
    [InlineData("Files/ECC.pfx", "4-tests", "1", Settings.DefaultKeyUse)]
    public void JwkCommand_Should_Be_Valid(string certificateFile, string certificatePassword, string keyId, string keyUse)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var certificateService = Substitute.For<ICertificateService>();
        var command = new JwkCommand(certificateService, consoleService)
        {
            CertificateFile = certificateFile,
            CertificatePassword = certificatePassword,
            KeyId = keyId,
            KeyUse = keyUse
        };
        var validator = new JwkCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("Files/FOO.pfx", "4-tests", "1", Settings.DefaultKeyUse)]
    [InlineData("Files/RSA.pfx", "4-tests", "", Settings.DefaultKeyUse)]
    [InlineData("Files/ECC.pfx", "4-tests", "1", "")]
    public void JwkCommand_Should_Not_Be_Valid(string certificateFile, string certificatePassword, string keyId, string keyUse)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var certificateService = Substitute.For<ICertificateService>();
        var command = new JwkCommand(certificateService, consoleService)
        {
            CertificateFile = certificateFile,
            CertificatePassword = certificatePassword,
            KeyId = keyId,
            KeyUse = keyUse
        };
        var validator = new JwkCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeFalse();
    }
}