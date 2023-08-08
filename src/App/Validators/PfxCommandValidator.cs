using App.Commands;
using App.Extensions;
using FluentValidation;

namespace App.Validators;

public abstract class PfxCommandValidator : AbstractValidator<PfxCommand>
{
    protected PfxCommandValidator()
    {
        RuleFor(x => x.CertificateFile)
            .Required();
        
        RuleFor(x => x.CertificatePassword)
            .Required();
        
        RuleFor(x => x.DnsName)
            .Required();

        RuleFor(x => x.ValidityInYears)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.OutputDirectory)
            .Required()
            .DirectoryExists();
    }
}