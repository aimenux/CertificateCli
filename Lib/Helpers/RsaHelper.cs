using System.Security.Cryptography;
using CertificateManager.Models;
using Lib.Configuration;

namespace Lib.Helpers;

public interface IRsaHelper
{
    void GenerateRsa(RsaParameters parameters);
}

public class RsaHelper : IRsaHelper
{
    private readonly ICertificateManagerWrapper _managerWrapper;

    public RsaHelper(ICertificateManagerWrapper managerWrapper)
    {
        _managerWrapper = managerWrapper;
    }

    public void GenerateRsa(RsaParameters parameters)
    {
        using var certificate = _managerWrapper.NewRsaSelfSignedCertificate(
            new DistinguishedName { CommonName = parameters.DnsName },
            GetBasicConstraints(),
            GetValidityPeriod(parameters),
            GetSubjectAlternativeName(parameters),
            GetEnhancedKeyUsages(),
            parameters.X509KeyUsageFlags,
            new RsaConfiguration { KeySize = parameters.KeySize }
        );

        var pfxBytes = _managerWrapper.ExportSelfSignedCertificatePfx(certificate, parameters.CertificatePassword);
        File.WriteAllBytes(parameters.CertificateFile, pfxBytes);
    }

    private static SubjectAlternativeName GetSubjectAlternativeName(RsaParameters parameters)
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

    private static ValidityPeriod GetValidityPeriod(RsaParameters parameters)
    {
        return new ValidityPeriod
        {
            ValidFrom = DateTimeOffset.UtcNow,
            ValidTo = DateTimeOffset.UtcNow.AddYears(parameters.ValidityInYears)
        };
    }

    private static OidCollection GetEnhancedKeyUsages()
    {
        const string tlsServerAuthOid = "1.3.6.1.5.5.7.3.1";
        const string tlsClientAuthOid = "1.3.6.1.5.5.7.3.2";

        var enhancedKeyUsages = new OidCollection
        {
            new Oid(tlsServerAuthOid),
            new Oid(tlsClientAuthOid),
        };

        return enhancedKeyUsages;
    }
}