using App.Services.Certificate;
using App.Services.Console;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

[Command(Name = "Ecc", FullName = "Generate ECC certificate", Description = "Generate ECC certificate.")]
[HelpOption]
public class EccCommand : PfxCommand
{
    public static int[] KeySizes { get; } = { 256, 384, 521 };
    
    public EccCommand(ICertificateService certificateService, IConsoleService consoleService) : base(certificateService, consoleService)
    {
        CertificateFile = $"ECC-{DateTime.Now:yyyyMMddTHHmmss}.pfx";
        KeySize = KeySizes[0];
    }

    protected override void Execute(CommandLineApplication app)
    {
        ConsoleService.RenderStatus(() =>
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
            CertificateService.GenerateEcc(parameters);
            ConsoleService.RenderPfx(parameters);
        });
    }
}