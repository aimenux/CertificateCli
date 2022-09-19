using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CertificateManager;
using CertificateManager.Models;

namespace Lib.Helpers
{
    public interface ICertificateManager
    {
        X509Certificate2 NewRsaSelfSignedCertificate(
            DistinguishedName distinguishedName,
            BasicConstraints basicConstraints,
            ValidityPeriod validityPeriod,
            SubjectAlternativeName subjectAlternativeName,
            OidCollection enhancedKeyUsages,
            X509KeyUsageFlags x509KeyUsageFlags,
            RsaConfiguration rsaConfiguration);

        X509Certificate2 NewEccSelfSignedCertificate(
            DistinguishedName distinguishedName,
            BasicConstraints basicConstraints,
            ValidityPeriod validityPeriod,
            SubjectAlternativeName subjectAlternativeName,
            OidCollection enhancedKeyUsages,
            X509KeyUsageFlags x509KeyUsageFlags,
            ECDsaConfiguration eccConfiguration);

        byte[] ExportSelfSignedCertificatePfx(X509Certificate2 certificate, string password);
    }

    public class CertificateManager : ICertificateManager
    {
        private readonly CreateCertificates _createCertificates;
        private readonly ImportExportCertificate _importExportCertificate;

        public CertificateManager(CreateCertificates createCertificates, ImportExportCertificate importExportCertificate)
        {
            _createCertificates = createCertificates ?? throw new ArgumentNullException(nameof(createCertificates));
            _importExportCertificate = importExportCertificate ?? throw new ArgumentNullException(nameof(importExportCertificate));
        }

        public X509Certificate2 NewRsaSelfSignedCertificate(
            DistinguishedName distinguishedName,
            BasicConstraints basicConstraints,
            ValidityPeriod validityPeriod,
            SubjectAlternativeName subjectAlternativeName,
            OidCollection enhancedKeyUsages,
            X509KeyUsageFlags x509KeyUsageFlags,
            RsaConfiguration rsaConfiguration)
        {
            return _createCertificates.NewRsaSelfSignedCertificate(distinguishedName, basicConstraints, validityPeriod, subjectAlternativeName, enhancedKeyUsages, x509KeyUsageFlags, rsaConfiguration);
        }

        public X509Certificate2 NewEccSelfSignedCertificate(
            DistinguishedName distinguishedName,
            BasicConstraints basicConstraints,
            ValidityPeriod validityPeriod,
            SubjectAlternativeName subjectAlternativeName,
            OidCollection enhancedKeyUsages,
            X509KeyUsageFlags x509KeyUsageFlags,
            ECDsaConfiguration eccConfiguration)
        {
            return _createCertificates.NewECDsaSelfSignedCertificate(distinguishedName, basicConstraints, validityPeriod, subjectAlternativeName, enhancedKeyUsages, x509KeyUsageFlags, eccConfiguration);
        }

        public byte[] ExportSelfSignedCertificatePfx(X509Certificate2 certificate, string password)
        {
            return _importExportCertificate.ExportSelfSignedCertificatePfx(password, certificate);
        }
    }
}
