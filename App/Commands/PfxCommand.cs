using System.ComponentModel.DataAnnotations;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

public abstract class PfxCommand : AbstractCommand
{
    protected readonly ICertificateHelper CertificateHelper;

    protected PfxCommand(ICertificateHelper certificateHelper, IConsoleHelper consoleHelper) : base(consoleHelper)
    {
        CertificateHelper = certificateHelper ?? throw new ArgumentNullException(nameof(certificateHelper));
    }

    [Option("-f|--file", "CertificateFile", CommandOptionType.SingleValue)]
    public string CertificateFile { get; set; } = $"Certificate-{DateTime.Now:yyyyMMddTHHmmss}.pfx";

    [Required]
    [Option("-p|--password", "CertificatePassword", CommandOptionType.SingleValue)]
    public string CertificatePassword { get; set; }

    [Option("-d|--dns", "DnsName", CommandOptionType.SingleValue)]
    public string DnsName { get; set; } = "localhost";

    [Option("-s|--size", "KeySize", CommandOptionType.SingleValue)]
    public int KeySize { get; set; }

    [Option("-v|--validity", "ValidityInYears", CommandOptionType.SingleValue)]
    public int ValidityInYears { get; set; } = 1;

    [Option("-o|--out", "OutputDirectory", CommandOptionType.SingleValue)]
    public string OutputDirectory { get; set; } = GetDefaultOutputDirectory();

    protected override bool HasValidOptions()
    {
        return !string.IsNullOrWhiteSpace(CertificateFile)
               && !string.IsNullOrWhiteSpace(CertificatePassword)
               && !string.IsNullOrWhiteSpace(DnsName)
               && ValidityInYears is >= 1 and <= 10
               && Directory.Exists(OutputDirectory)
               && IsCertificateFilePathValid()
               && IsValidKeySize();
    }

    private bool IsCertificateFilePathValid()
    {
        var fullname = Path.Combine(OutputDirectory, CertificateFile);
        return !File.Exists(fullname);
    }

    private static string GetDefaultOutputDirectory()
    {
        const string defaultDir = @"C:/Logs";

        return Directory.Exists(defaultDir)
            ? defaultDir
            : Path.GetFullPath("./");
    }

    protected abstract bool IsValidKeySize();
}