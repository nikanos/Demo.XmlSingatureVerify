using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Demo.XmlSignatureVerify.Lib
{
    public class CertificateRetriever : ICertificateRetriever
    {
        public X509Certificate2 GetCertificateFromTrustedStore(string thumbprint)
        {
            if (thumbprint == null)
                throw new ArgumentNullException(nameof(thumbprint));
            if (thumbprint == string.Empty)
                throw new ArgumentException("Should not be empty", nameof(thumbprint));

            X509Certificate2 foundCertificate;
            foundCertificate = FindCertificateByThumbprint(thumbprint, StoreName.TrustedPeople, StoreLocation.CurrentUser);
            if (foundCertificate == null)
                foundCertificate = FindCertificateByThumbprint(thumbprint, StoreName.TrustedPeople, StoreLocation.LocalMachine);

            return foundCertificate;
        }

        public bool IsCertificateInTrustedStore(string thumbprint)
        {
            if (thumbprint == null)
                throw new ArgumentNullException(nameof(thumbprint));
            if (thumbprint == string.Empty)
                throw new ArgumentException("Should not be empty", nameof(thumbprint));

            return GetCertificateFromTrustedStore(thumbprint) != null;
        }

        private X509Certificate2 FindCertificateByThumbprint(string thumbprint, StoreName storeName, StoreLocation storeLocation)
        {
            using (var store = new X509Store(storeName, storeLocation))
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificateCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, validOnly: true);
                X509Certificate2 certificate = certificateCollection.OfType<X509Certificate2>().FirstOrDefault();
                store.Close();
                return certificate;
            }
        }
    }
}
