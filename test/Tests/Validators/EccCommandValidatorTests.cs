using App.Commands;
using App.Services.Certificate;
using App.Services.Console;
using App.Validators;
using FluentAssertions;
using NSubstitute;

namespace Tests.Validators;

public class EccCommandValidatorTests
{
    [Theory]
    [InlineData("Ecc1.pfx", "Pass1", 256, 1)]
    [InlineData("Ecc2.pfx", "Pass2", 384, 2)]
    [InlineData("Ecc3.pfx", "Pass3", 521, 3)]
    public void EccCommand_Should_Be_Valid(string certificateFile, string certificatePassword, int keySize, int years)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var certificateService = Substitute.For<ICertificateService>();
        var command = new EccCommand(certificateService, consoleService)
        {
            CertificateFile = certificateFile,
            CertificatePassword = certificatePassword,
            ValidityInYears = years,
            KeySize = keySize
        };
        var validator = new EccCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("", "Pass", 256, 1)]
    [InlineData("Ecc.pfx", "", 384, 2)]
    [InlineData("Ecc.pfx", "Pass", 100, 3)]
    [InlineData("Ecc.pfx", "Pass", 521, 0)]
    [InlineData("Ecc.pfx", "Pass", 521, -1)]
    [InlineData("Ecc.pfx", "Pass", 521, 1000)]
    public void EccCommand_Should_Not_Be_Valid(string certificateFile, string certificatePassword, int keySize, int years)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var certificateService = Substitute.For<ICertificateService>();
        var command = new EccCommand(certificateService, consoleService)
        {
            CertificateFile = certificateFile,
            CertificatePassword = certificatePassword,
            ValidityInYears = years,
            KeySize = keySize
        };
        var validator = new EccCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeFalse();
    }
}