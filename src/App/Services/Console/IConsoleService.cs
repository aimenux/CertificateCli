using App.Services.Certificate;
using App.Validators;

namespace App.Services.Console;

public interface IConsoleService
{
    void RenderTitle(string text);
    void RenderVersion(string version);
    void CopyTextToClipboard(string text);
    void RenderSettingsFile(string filepath);
    void RenderUserSecretsFile(string filepath);
    void RenderStatus(Action action);
    bool GetYesOrNoAnswer(string text, bool defaultAnswer);
    void RenderValidationErrors(ValidationErrors validationErrors);
    void RenderPfx(PfxParameters parameters);
    void RenderJwk(JwkParameters parameters, string jwk);
    void RenderException(Exception exception);
}