using App.Commands;
using App.Services.Certificate;
using App.Services.Console;
using App.Validators;
using FluentAssertions;
using NSubstitute;

namespace Tests.Validators;

public class RsaCommandValidatorTests
{
    [Theory]
    [InlineData("Rsa1.pfx", "Pass1", 2048, 1)]
    [InlineData("Rsa2.pfx", "Pass2", 3072, 2)]
    [InlineData("Rsa3.pfx", "Pass3", 7680, 3)]
    public void RsaCommand_Should_Be_Valid(string certificateFile, string certificatePassword, int keySize, int years)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var certificateService = Substitute.For<ICertificateService>();
        var command = new RsaCommand(certificateService, consoleService)
        {
            CertificateFile = certificateFile,
            CertificatePassword = certificatePassword,
            ValidityInYears = years,
            KeySize = keySize
        };
        var validator = new RsaCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("", "Pass", 2048, 1)]
    [InlineData("Rsa.pfx", "", 3072, 2)]
    [InlineData("Rsa.pfx", "Pass", 100, 3)]
    [InlineData("Rsa.pfx", "Pass", 7680, 0)]
    [InlineData("Rsa.pfx", "Pass", 7680, -1)]
    [InlineData("Rsa.pfx", "Pass", 7680, 1000)]
    public void RsaCommand_Should_Not_Be_Valid(string certificateFile, string certificatePassword, int keySize, int years)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var certificateService = Substitute.For<ICertificateService>();
        var command = new RsaCommand(certificateService, consoleService)
        {
            CertificateFile = certificateFile,
            CertificatePassword = certificatePassword,
            ValidityInYears = years,
            KeySize = keySize
        };
        var validator = new RsaCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeFalse();
    }
}