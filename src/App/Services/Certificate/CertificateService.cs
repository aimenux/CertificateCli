using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using App.Configuration;
using CertificateManager;
using CertificateManager.Models;
using Microsoft.Extensions.Options;

namespace App.Services.Certificate;

public class CertificateService : ICertificateService
{
    private readonly ICertificateHelper _helper;
    private readonly IOptions<Settings> _options;

    public CertificateService(ICertificateHelper helper, IOptions<Settings> options)
    {
        _helper = helper ?? throw new ArgumentNullException(nameof(helper));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public void GenerateRsa(PfxParameters parameters)
    {
        var pfxBytes = _helper.CreateRsaSelfSignedCertificate(
            new DistinguishedName { CommonName = parameters.DnsName },
            GetBasicConstraints(),
            GetValidityPeriod(parameters),
            GetSubjectAlternativeName(parameters),
            GetEnhancedKeyUsages(),
            GetKeyUsageFlags(),
            new RsaConfiguration { KeySize = parameters.KeySize },
            parameters.CertificatePassword
        );

        File.WriteAllBytes(parameters.CertificateFullPath, pfxBytes);
    }

    public void GenerateEcc(PfxParameters parameters)
    {
        var pfxBytes = _helper.CreateEccSelfSignedCertificate(
            new DistinguishedName { CommonName = parameters.DnsName },
            GetBasicConstraints(),
            GetValidityPeriod(parameters),
            GetSubjectAlternativeName(parameters),
            GetEnhancedKeyUsages(),
            GetKeyUsageFlags(),
            new ECDsaConfiguration { KeySize = parameters.KeySize },
            parameters.CertificatePassword
        );

        File.WriteAllBytes(parameters.CertificateFullPath, pfxBytes);
    }

    public string GenerateJwk(JwkParameters parameters)
    {
        if (_helper.IsRsaCertificate(parameters.CertificateFile, parameters.CertificatePassword))
        {
            return _helper.CreateRsaJwk(
                parameters.KeyId,
                parameters.KeyUse,
                parameters.CertificateFile,
                parameters.CertificatePassword
            );
        }
        
        if (_helper.IsEccCertificate(parameters.CertificateFile, parameters.CertificatePassword))
        {
            return _helper.CreateEccJwk(
                parameters.KeyId,
                parameters.KeyUse,
                parameters.CertificateFile,
                parameters.CertificatePassword
            );
        }
        
        throw new InvalidOperationException($"Unsupported certificate: '{parameters.CertificateFile}' is not either RSA nor ECC.");
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

    private X509KeyUsageFlags GetKeyUsageFlags()
    {
        var keyUsageFlags = _options.Value.KeyUsageFlags;

        if (keyUsageFlags?.Any() == true)
        {
            return keyUsageFlags
                .Select(Enum.Parse<X509KeyUsageFlags>)
                .Aggregate((x, y) => x | y);
        }

        return X509KeyUsageFlags.DigitalSignature
               | X509KeyUsageFlags.NonRepudiation
               | X509KeyUsageFlags.KeyCertSign;
    }
}