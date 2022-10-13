using System.Security.Cryptography.X509Certificates;

namespace Demo.XmlSignatureVerify.Lib
{
    public interface ICertificateRequester
    {
        bool IsCertificateInTrustedStore(string thumbprint);
        X509Certificate2 GetCertificateFromTrustedStore(string thumbprint);
    }
}
