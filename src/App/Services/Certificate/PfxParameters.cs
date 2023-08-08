namespace App.Services.Certificate;

public class PfxParameters
{
    public string DnsName { get; init; }

    public int KeySize { get; init; }

    public int ValidityInYears { get; init; }

    public string CertificateFile { get; init; }

    public string CertificatePassword { get; init; }

    public string OutputDirectory { get; init; }

    public string CertificateFullPath => Path.GetFullPath(Path.Combine(OutputDirectory, CertificateFile));

    public PfxType PfxType { get; init; }
}

public enum PfxType
{
    Rsa,
    Ecc
}