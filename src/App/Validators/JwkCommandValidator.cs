using App.Commands;
using App.Extensions;
using FluentValidation;

namespace App.Validators;

public class JwkCommandValidator : AbstractValidator<JwkCommand>
{
    public JwkCommandValidator()
    {
        RuleFor(x => x.CertificateFile)
            .Required()
            .FileExists();
        
        RuleFor(x => x.KeyId)
            .Required();
        
        RuleFor(x => x.KeyUse)
            .Required();
    }
}