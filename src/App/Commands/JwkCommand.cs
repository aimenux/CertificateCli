using App.Configuration;
using App.Services.Certificate;
using App.Services.Console;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

[Command(Name = "Jwk", FullName = "Generate Json Web Key (JWK)", Description = "Generate Json Web Key (JWK).")]
[HelpOption]
public class JwkCommand : AbstractCommand
{
    private readonly ICertificateService _certificateService;

    public JwkCommand(ICertificateService certificateService, IConsoleService consoleService) : base(consoleService)
    {
        _certificateService = certificateService ?? throw new ArgumentNullException(nameof(certificateService));
    }

    [Option("-f|--file", "CertificateFile", CommandOptionType.SingleValue)]
    public string CertificateFile { get; init; }

    [Option("-p|--password", "CertificatePassword", CommandOptionType.SingleValue)]
    public string CertificatePassword { get; init; }

    [Option("-id|--keyId", "KeyId", CommandOptionType.SingleValue)]
    public string KeyId { get; init; } = Guid.NewGuid().ToString("N");

    [Option("-use|--keyUse", "KeyUse", CommandOptionType.SingleValue)]
    public string KeyUse { get; init; } = Settings.DefaultKeyUse;

    protected override void Execute(CommandLineApplication app)
    {
        var parameters = new JwkParameters
        {
            KeyId = KeyId,
            KeyUse = KeyUse,
            CertificateFile = CertificateFile,
            CertificatePassword = CertificatePassword
        };

        var jwk = _certificateService.GenerateJwk(parameters);
        ConsoleService.RenderJwk(parameters, jwk);
        ConsoleService.CopyTextToClipboard(jwk);
    }
}