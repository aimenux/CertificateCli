using App.Services.Certificate;
using App.Services.Console;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

[Command(Name = "Rsa", FullName = "Generate RSA certificate", Description = "Generate RSA certificate.")]
[HelpOption]
public class RsaCommand : PfxCommand
{
    public static int[] KeySizes { get; } = { 2048, 3072, 7680 };
    
    public RsaCommand(ICertificateService certificateService, IConsoleService consoleService) : base(certificateService, consoleService)
    {
        CertificateFile = $"RSA-{DateTime.Now:yyyyMMddTHHmmss}.pfx";
        KeySize = KeySizes.First();
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
            PfxType = PfxType.Rsa
        };

        ConsoleService.RenderStatus(() =>
        {
            CertificateService.GenerateRsa(parameters);
            ConsoleService.RenderPfx(parameters);
        });
    }
}