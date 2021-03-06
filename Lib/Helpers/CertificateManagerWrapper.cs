using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CertificateManager;
using CertificateManager.Models;

namespace Lib.Helpers
{
    public interface ICertificateManagerWrapper
    {
        X509Certificate2 NewRsaSelfSignedCertificate(
            DistinguishedName distinguishedName,
            BasicConstraints basicConstraints,
            ValidityPeriod validityPeriod,
            SubjectAlternativeName subjectAlternativeName,
            OidCollection enhancedKeyUsages,
            X509KeyUsageFlags x509KeyUsageFlags,
            RsaConfiguration rsaConfiguration);

        byte[] ExportSelfSignedCertificatePfx(X509Certificate2 certificate, string password);
    }

    public class CertificateManagerWrapper : ICertificateManagerWrapper
    {
        private readonly CreateCertificates _createCertificates;
        private readonly ImportExportCertificate _importExportCertificate;

        public CertificateManagerWrapper(CreateCertificates createCertificates, ImportExportCertificate importExportCertificate)
        {
            _createCertificates = createCertificates;
            _importExportCertificate = importExportCertificate;
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

        public byte[] ExportSelfSignedCertificatePfx(X509Certificate2 certificate, string password)
        {
            return _importExportCertificate.ExportSelfSignedCertificatePfx(password, certificate);
        }
    }
}
