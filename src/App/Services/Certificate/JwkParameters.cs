namespace App.Services.Certificate;

public class JwkParameters
{
    public string KeyId { get; init; }

    public string KeyUse { get; init; }

    public string CertificateFile { get; init; }

    public string CertificatePassword { get; init; }
}