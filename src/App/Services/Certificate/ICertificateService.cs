namespace App.Services.Certificate;

public interface ICertificateService
{
    void GenerateRsa(PfxParameters parameters);
    void GenerateEcc(PfxParameters parameters);
    string GenerateJwk(JwkParameters parameters);
}