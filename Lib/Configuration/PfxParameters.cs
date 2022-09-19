namespace Lib.Configuration;

public class PfxParameters
{
    public string DnsName { get; set; }

    public int KeySize { get; set; }

    public int ValidityInYears { get; set; }

    public string CertificateFile { get; set; }

    public string CertificatePassword { get; set; }

    public string OutputDirectory { get; set; }

    public string CertificateFullPath => Path.GetFullPath(Path.Combine(OutputDirectory, CertificateFile));

    public PfxType PfxType { get; set; }
}