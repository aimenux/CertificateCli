using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Lib.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Lib.Helpers;

public interface IJwkHelper
{
    string GenerateJwk(JwkParameters parameters);
}

public class JwkHelper : IJwkHelper
{
    public string GenerateJwk(JwkParameters parameters)
    {
        var certificate = new X509Certificate2(parameters.CertificateFile, parameters.CertificatePassword);
        var rsaKey = certificate.GetRSAPublicKey();
        if (rsaKey is null)
        {
            throw new InvalidOperationException("Unsupported certificate: public key is not RSA");
        }

        var securityAlgorithm = GetSecurityAlgorithm(certificate);
        var securityKey = new X509SecurityKey(certificate, parameters.KeyId);
        var jsonWebKey = JsonWebKeyConverter.ConvertFromX509SecurityKey(securityKey, true);
        jsonWebKey.Use ??= parameters.KeyUse;
        jsonWebKey.Alg ??= securityAlgorithm;

        var jwk = new
        {
            Kty = jsonWebKey.Kty,
            Use = jsonWebKey.Use,
            Alg = jsonWebKey.Alg,
            Kid = jsonWebKey.Kid,
            N = jsonWebKey.N,
            E = jsonWebKey.E
        };

        var json = JsonSerializer.Serialize(jwk);
        return json;
    }

    private static string GetSecurityAlgorithm(X509Certificate2 certificate)
    {
        var algorithm = certificate.SignatureAlgorithm;
        return algorithm.Value switch
        {
            "1.2.840.113549.1.1.11" => SecurityAlgorithms.RsaSha256,
            "1.2.840.113549.1.1.12" => SecurityAlgorithms.RsaSha384,
            "1.2.840.113549.1.1.13" => SecurityAlgorithms.RsaSha512,
            _ => throw new InvalidOperationException($"Unsupported algorithm {algorithm.Value} ({algorithm.FriendlyName})"),
        };
    }
}