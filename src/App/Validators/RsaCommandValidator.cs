using App.Commands;
using App.Extensions;

namespace App.Validators;

public class RsaCommandValidator : PfxCommandValidator
{
    public RsaCommandValidator()
    {
        RuleFor(x => x.KeySize)
            .In(RsaCommand.KeySizes);
    }
}