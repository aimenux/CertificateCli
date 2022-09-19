using System.ComponentModel.DataAnnotations;
using Lib.Configuration;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

[Command(Name = "Jwk", FullName = "Generate Json Web Key (JWK)", Description = "Generate Json Web Key (JWK).")]
[VersionOptionFromMember(MemberName = nameof(GetVersion))]
[HelpOption]
public class JwkCommand : AbstractCommand
{
    private readonly IJwkHelper _jwkHelper;

    public JwkCommand(IJwkHelper jwkHelper, IConsoleHelper consoleHelper) : base(consoleHelper)
    {
        _jwkHelper = jwkHelper ?? throw new ArgumentNullException(nameof(jwkHelper));
    }

    [Required]
    [Option("-f|--file", "CertificateFile", CommandOptionType.SingleValue)]
    public string CertificateFile { get; set; }

    [Required]
    [Option("-p|--password", "CertificatePassword", CommandOptionType.SingleValue)]
    public string CertificatePassword { get; set; }

    [Option("-id|--keyId", "KeyId", CommandOptionType.SingleValue)]
    public string KeyId { get; set; } = "1";

    [Option("-use|--keyUse", "KeyUse", CommandOptionType.SingleValue)]
    public string KeyUse { get; set; } = "Sig";

    protected override void Execute(CommandLineApplication app)
    {
        var parameters = new JwkParameters
        {
            KeyId = KeyId,
            KeyUse = KeyUse,
            CertificateFile = CertificateFile,
            CertificatePassword = CertificatePassword
        };

        var jwk = _jwkHelper.GenerateJwk(parameters);
        ConsoleHelper.RenderJwk(parameters, jwk);
        ConsoleHelper.CopyTextToClipboard(jwk);
    }

    protected override bool HasValidOptions()
    {
        return File.Exists(CertificateFile)
            && !string.IsNullOrWhiteSpace(CertificatePassword)
            && !string.IsNullOrWhiteSpace(KeyId)
            && !string.IsNullOrWhiteSpace(KeyUse);
    }

    protected static string GetVersion() => GetVersion(typeof(JwkCommand));
}