using System.ComponentModel.DataAnnotations;
using App.Configuration;
using App.Services.Certificate;
using App.Services.Console;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

public abstract class PfxCommand : AbstractCommand
{
    protected readonly ICertificateService CertificateService;

    protected PfxCommand(ICertificateService certificateService, IConsoleService consoleService) : base(consoleService)
    {
        CertificateService = certificateService ?? throw new ArgumentNullException(nameof(certificateService));
    }

    [Option("-f|--file", "CertificateFile", CommandOptionType.SingleValue)]
    public string CertificateFile { get; init; } = $"Certificate-{DateTime.Now:yyyyMMddTHHmmss}.pfx";

    [Option("-p|--password", "CertificatePassword", CommandOptionType.SingleValue)]
    public string CertificatePassword { get; init; }

    [Option("-d|--dns", "DnsName", CommandOptionType.SingleValue)]
    public string DnsName { get; init; } = Settings.DefaultDnsName;

    [Option("-s|--size", "KeySize", CommandOptionType.SingleValue)]
    public int KeySize { get; init; }

    [Option("-v|--validity", "ValidityInYears", CommandOptionType.SingleValue)]
    public int ValidityInYears { get; init; } = 1;

    [Option("-o|--out", "OutputDirectory", CommandOptionType.SingleValue)]
    public string OutputDirectory { get; init; } = Settings.GetDefaultOutputDirectory();
}