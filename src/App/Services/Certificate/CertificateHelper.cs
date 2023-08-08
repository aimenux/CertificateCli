using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using CertificateManager;
using CertificateManager.Models;
using Microsoft.IdentityModel.Tokens;

namespace App.Services.Certificate;

public class CertificateHelper : ICertificateHelper
{
    private readonly CreateCertificates _createCertificates;
    private readonly ImportExportCertificate _importExportCertificate;

    public CertificateHelper(CreateCertificates createCertificates, ImportExportCertificate importExportCertificate)
    {
        _createCertificates = createCertificates ?? throw new ArgumentNullException(nameof(createCertificates));
        _importExportCertificate = importExportCertificate ?? throw new ArgumentNullException(nameof(importExportCertificate));
    }

    public bool IsRsaCertificate(string certificateFile, string certificatePassword)
    {
        using var certificate = string.IsNullOrWhiteSpace(certificatePassword)
            ? new X509Certificate2(certificateFile)
            : new X509Certificate2(certificateFile, certificatePassword);
        using var publicKey = certificate.GetRSAPublicKey();
        return publicKey is not null;
    }

    public bool IsEccCertificate(string certificateFile, string certificatePassword)
    {
        using var certificate = string.IsNullOrWhiteSpace(certificatePassword)
            ? new X509Certificate2(certificateFile)
            : new X509Certificate2(certificateFile, certificatePassword);
        using var publicKey = certificate.GetECDsaPublicKey();
        return publicKey is not null;
    }

    public byte[] CreateRsaSelfSignedCertificate(
        DistinguishedName distinguishedName,
        BasicConstraints basicConstraints,
        ValidityPeriod validityPeriod,
        SubjectAlternativeName subjectAlternativeName,
        OidCollection enhancedKeyUsages,
        X509KeyUsageFlags x509KeyUsageFlags,
        RsaConfiguration rsaConfiguration,
        string certificatePassword)
    {
        using var certificate = _createCertificates.NewRsaSelfSignedCertificate(
            distinguishedName,
            basicConstraints,
            validityPeriod,
            subjectAlternativeName,
            enhancedKeyUsages,
            x509KeyUsageFlags,
            rsaConfiguration);
        var bytes = _importExportCertificate.ExportSelfSignedCertificatePfx(certificatePassword, certificate);
        return bytes;
    }

    public byte[] CreateEccSelfSignedCertificate(
        DistinguishedName distinguishedName,
        BasicConstraints basicConstraints,
        ValidityPeriod validityPeriod,
        SubjectAlternativeName subjectAlternativeName,
        OidCollection enhancedKeyUsages,
        X509KeyUsageFlags x509KeyUsageFlags,
        ECDsaConfiguration eccConfiguration,
        string certificatePassword)
    {
        using var certificate = _createCertificates.NewECDsaSelfSignedCertificate(
            distinguishedName,
            basicConstraints,
            validityPeriod,
            subjectAlternativeName,
            enhancedKeyUsages,
            x509KeyUsageFlags,
            eccConfiguration);
        var bytes = _importExportCertificate.ExportSelfSignedCertificatePfx(certificatePassword, certificate);
        return bytes;
    }

    public string CreateRsaJwk(string keyId, string keyUse, string certificateFile, string certificatePassword)
    {
        using var certificate = string.IsNullOrWhiteSpace(certificatePassword)
            ? new X509Certificate2(certificateFile)
            : new X509Certificate2(certificateFile, certificatePassword);
            
        using var publicKey = certificate.GetRSAPublicKey();
        if (publicKey is null)
        {
            throw new InvalidOperationException($"Unsupported certificate: '{certificateFile}' is not RSA.");
        }
            
        var securityKey = new RsaSecurityKey(publicKey);
        var securityAlgorithm = GetSecurityAlgorithm(certificate);
        var jsonWebKey = JsonWebKeyConverter.ConvertFromRSASecurityKey(securityKey);

        var jwk = new
        {
            Kty = jsonWebKey.Kty,
            Use = keyUse ?? jsonWebKey.Use,
            Alg = securityAlgorithm ?? jsonWebKey.Alg,
            Kid = keyId ?? jsonWebKey.Kid,
            E = jsonWebKey.E,
            N = jsonWebKey.N,
        };

        return JsonSerializer.Serialize(jwk);
    }

    public string CreateEccJwk(string keyId, string keyUse, string certificateFile, string certificatePassword)
    {
        using var certificate = string.IsNullOrWhiteSpace(certificatePassword)
            ? new X509Certificate2(certificateFile)
            : new X509Certificate2(certificateFile, certificatePassword);
            
        using var publicKey = certificate.GetECDsaPublicKey();
        if (publicKey is null)
        {
            throw new InvalidOperationException($"Unsupported certificate: '{certificateFile}' is not ECC.");
        }
            
        var securityKey = new ECDsaSecurityKey(publicKey);
        var securityAlgorithm = GetSecurityAlgorithm(certificate);
        var jsonWebKey = JsonWebKeyConverter.ConvertFromECDsaSecurityKey(securityKey);

        var jwk = new
        {
            Kty = jsonWebKey.Kty,
            Use = keyUse ?? jsonWebKey.Use,
            Alg = securityAlgorithm ?? jsonWebKey.Alg,
            Kid = keyId ?? jsonWebKey.Kid,
            Crv = jsonWebKey.Crv,
            X = jsonWebKey.X,
            Y = jsonWebKey.Y
        };

        return JsonSerializer.Serialize(jwk);
    }

    private static string GetSecurityAlgorithm(X509Certificate2 certificate)
    {
        var algorithm = certificate.SignatureAlgorithm;
        return algorithm.Value switch
        {
            "1.2.840.113549.1.1.11" => SecurityAlgorithms.RsaSha256,
            "1.2.840.113549.1.1.12" => SecurityAlgorithms.RsaSha384,
            "1.2.840.113549.1.1.13" => SecurityAlgorithms.RsaSha512,
            "1.2.840.10045.4.3.2" => SecurityAlgorithms.EcdsaSha256,
            "1.2.840.10045.4.3.3" => SecurityAlgorithms.EcdsaSha384,
            "1.2.840.10045.4.3.4" => SecurityAlgorithms.EcdsaSha512,
            _ => null
        };
    }
}