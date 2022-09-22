using Lib.Configuration;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

[Command(Name = "Ecc", FullName = "Generate ECC certificate", Description = "Generate ECC certificate.")]
[VersionOptionFromMember(MemberName = nameof(GetVersion))]
[HelpOption]
public class EccCommand : PfxCommand
{
    public EccCommand(ICertificateHelper certificateHelper, IConsoleHelper consoleHelper) : base(certificateHelper, consoleHelper)
    {
        KeySize = 256;
        CertificateFile = $"ECC-{DateTime.Now:yyyyMMddTHHmmss}.pfx";
    }

    protected override void Execute(CommandLineApplication app)
    {
        var parameters = new PfxParameters
        {
            DnsName = DnsName,
            KeySize = KeySize,
            ValidityInYears = ValidityInYears,
            CertificateFile = CertificateFile,
            CertificatePassword = CertificatePassword,
            OutputDirectory = OutputDirectory,
            PfxType = PfxType.Ecc
        };

        ConsoleHelper.RenderStatus(() =>
        {
            CertificateHelper.GenerateEcc(parameters);
            ConsoleHelper.RenderPfx(parameters);
        });
    }

    protected override bool IsValidKeySize()
    {
        return KeySize is 160 or 224 or 256 or 384 or 521;
    }

    protected static string GetVersion() => GetVersion(typeof(EccCommand));
}