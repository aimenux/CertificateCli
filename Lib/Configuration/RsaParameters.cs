using System.Security.Cryptography.X509Certificates;

namespace Lib.Configuration;

public class RsaParameters
{
    public string DnsName { get; set; }

    public int KeySize { get; set; }

    public int ValidityInYears { get; set; }

    public string CertificateFile { get; set; }

    public string CertificatePassword { get; set; }

    public X509KeyUsageFlags X509KeyUsageFlags => X509KeyUsageFlags.DigitalSignature;
}