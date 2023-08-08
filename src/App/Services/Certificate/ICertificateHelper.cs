using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CertificateManager.Models;

namespace App.Services.Certificate;

public interface ICertificateHelper
{
    bool IsRsaCertificate(string certificateFile, string certificatePassword);
    
    bool IsEccCertificate(string certificateFile, string certificatePassword);
    
    byte[] CreateRsaSelfSignedCertificate(
        DistinguishedName distinguishedName,
        BasicConstraints basicConstraints,
        ValidityPeriod validityPeriod,
        SubjectAlternativeName subjectAlternativeName,
        OidCollection enhancedKeyUsages,
        X509KeyUsageFlags x509KeyUsageFlags,
        RsaConfiguration rsaConfiguration,
        string certificatePassword);

    byte[] CreateEccSelfSignedCertificate(
        DistinguishedName distinguishedName,
        BasicConstraints basicConstraints,
        ValidityPeriod validityPeriod,
        SubjectAlternativeName subjectAlternativeName,
        OidCollection enhancedKeyUsages,
        X509KeyUsageFlags x509KeyUsageFlags,
        ECDsaConfiguration eccConfiguration,
        string certificatePassword);
    
    string CreateRsaJwk(
        string keyId,
        string keyUse,
        string certificateFile,
        string certificatePassword);
    
    string CreateEccJwk(
        string keyId,
        string keyUse,
        string certificateFile,
        string certificatePassword);
}