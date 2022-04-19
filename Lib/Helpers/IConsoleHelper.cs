using Lib.Configuration;

namespace Lib.Helpers
{
    public interface IConsoleHelper
    {
        void RenderTitle(string text);

        void CopyTextToClipboard(string text);

        void RenderSettingsFile(string filepath);

        void RenderRsa(RsaParameters parameters);

        void RenderJwk(JwkParameters parameters, string jwk);

        void RenderException(Exception exception);
    }
}
