using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CertificateManager;
using CertificateManager.Models;
using Lib.Configuration;

namespace Lib.Helpers;

public interface ICertificateHelper
{
    void GenerateRsa(PfxParameters parameters);
    void GenerateEcc(PfxParameters parameters);
}

public class CertificateHelper : ICertificateHelper
{
    private readonly ICertificateManager _manager;

    public CertificateHelper(ICertificateManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager));
    }

    public void GenerateRsa(PfxParameters parameters)
    {
        using var certificate = _manager.NewRsaSelfSignedCertificate(
            new DistinguishedName { CommonName = parameters.DnsName },
            GetBasicConstraints(),
            GetValidityPeriod(parameters),
            GetSubjectAlternativeName(parameters),
            GetEnhancedKeyUsages(),
            GetKeyUsageFlags(),
            new RsaConfiguration { KeySize = parameters.KeySize }
        );

        var pfxBytes = _manager.ExportSelfSignedCertificatePfx(certificate, parameters.CertificatePassword);
        File.WriteAllBytes(parameters.CertificateFullPath, pfxBytes);
    }

    public void GenerateEcc(PfxParameters parameters)
    {
        using var certificate = _manager.NewEccSelfSignedCertificate(
            new DistinguishedName { CommonName = parameters.DnsName },
            GetBasicConstraints(),
            GetValidityPeriod(parameters),
            GetSubjectAlternativeName(parameters),
            GetEnhancedKeyUsages(),
            GetKeyUsageFlags(),
            new ECDsaConfiguration { KeySize = parameters.KeySize }
        );

        var pfxBytes = _manager.ExportSelfSignedCertificatePfx(certificate, parameters.CertificatePassword);
        File.WriteAllBytes(parameters.CertificateFullPath, pfxBytes);
    }

    private static SubjectAlternativeName GetSubjectAlternativeName(PfxParameters parameters)
    {
        return new SubjectAlternativeName
        {
            DnsName = new List<string>
            {
                parameters.DnsName
            }
        };
    }

    private static BasicConstraints GetBasicConstraints()
    {
        return new BasicConstraints
        {
            CertificateAuthority = false,
            HasPathLengthConstraint = false,
            PathLengthConstraint = 0,
            Critical = false
        };
    }

    private static ValidityPeriod GetValidityPeriod(PfxParameters parameters)
    {
        return new ValidityPeriod
        {
            ValidFrom = DateTimeOffset.UtcNow,
            ValidTo = DateTimeOffset.UtcNow.AddYears(parameters.ValidityInYears)
        };
    }

    private static OidCollection GetEnhancedKeyUsages()
    {
        var enhancedKeyUsages = new OidCollection
        {
            OidLookup.ClientAuthentication,
            OidLookup.ServerAuthentication,
        };

        return enhancedKeyUsages;
    }

    private static X509KeyUsageFlags GetKeyUsageFlags()
    {
        return X509KeyUsageFlags.DigitalSignature
               | X509KeyUsageFlags.KeyEncipherment
               | X509KeyUsageFlags.NonRepudiation
               | X509KeyUsageFlags.KeyCertSign;
    }
}