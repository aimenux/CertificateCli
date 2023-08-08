using App.Commands;
using App.Exceptions;
using App.Services.Certificate;
using App.Services.Console;
using App.Validators;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using NSubstitute;

namespace Tests.Validators;

public class ToolCommandValidatorTests
{
    [Fact]
    public void Should_Get_ValidationErrors_For_ToolCommand()
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var command = new ToolCommand(consoleService);

        // act
        var validationErrors = ToolCommandValidator.Validate(command);

        // assert
        validationErrors.Should().NotBeNull();
    }
    
    [Fact]
    public void Should_Get_ValidationErrors_For_RsaCommand()
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var certificateService = Substitute.For<ICertificateService>();
        var command = new RsaCommand(certificateService, consoleService);

        // act
        var validationErrors = ToolCommandValidator.Validate(command);

        // assert
        validationErrors.Should().NotBeNull();
    }
    
    [Fact]
    public void Should_Get_ValidationErrors_For_EccCommand()
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var certificateService = Substitute.For<ICertificateService>();
        var command = new EccCommand(certificateService, consoleService);

        // act
        var validationErrors = ToolCommandValidator.Validate(command);

        // assert
        validationErrors.Should().NotBeNull();
    }
    
    [Fact]
    public void Should_Get_ValidationErrors_For_JwkCommand()
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var certificateService = Substitute.For<ICertificateService>();
        var command = new JwkCommand(certificateService, consoleService);

        // act
        var validationErrors = ToolCommandValidator.Validate(command);

        // assert
        validationErrors.Should().NotBeNull();
    }
    
    [Fact]
    public void Should_Throw_Exception_For_Unexpected_Commands()
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var command = new UnexpectedCommand(consoleService);

        // act
        var validateFunc = () => ToolCommandValidator.Validate(command);

        // assert
        validateFunc.Should().Throw<CertificateCliException>();
    }

    private class UnexpectedCommand : AbstractCommand
    {
        public UnexpectedCommand(IConsoleService consoleService) : base(consoleService)
        {
        }

        protected override void Execute(CommandLineApplication app)
        {
        }
    }
}