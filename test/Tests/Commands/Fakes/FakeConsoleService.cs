using App.Services.Certificate;
using App.Services.Console;
using App.Validators;

namespace Tests.Commands.Fakes;

public class FakeConsoleService : IConsoleService
{
    public void RenderTitle(string text)
    {
    }

    public void RenderVersion(string version)
    {
    }

    public void CopyTextToClipboard(string text)
    {
    }

    public void RenderSettingsFile(string filepath)
    {
    }

    public void RenderUserSecretsFile(string filepath)
    {
    }

    public void RenderStatus(Action action)
    {
        action?.Invoke();
    }

    public bool GetYesOrNoAnswer(string text, bool defaultAnswer)
    {
        return defaultAnswer;
    }

    public void RenderValidationErrors(ValidationErrors validationErrors)
    {
    }

    public void RenderPfx(PfxParameters parameters)
    {
    }

    public void RenderJwk(JwkParameters parameters, string jwk)
    {
    }

    public void RenderException(Exception exception)
    {
    }
}