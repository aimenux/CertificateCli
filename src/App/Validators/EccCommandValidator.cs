using App.Commands;
using App.Extensions;

namespace App.Validators;

public class EccCommandValidator : PfxCommandValidator
{
    public EccCommandValidator()
    {
        RuleFor(x => x.KeySize)
            .In(EccCommand.KeySizes);
    }
}