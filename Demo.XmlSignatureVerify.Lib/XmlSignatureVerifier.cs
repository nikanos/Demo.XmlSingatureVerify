using Demo.XmlSignatureVerify.Lib.Exceptions;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Demo.XmlSignatureVerify.Lib
{
    public class XmlSignatureVerifier
    {
        private readonly ICertificateRetriever _certificateRetriever;
        private readonly ISignedXmlFactory _signedXmlFactory;

        public XmlSignatureVerifier(ICertificateRetriever certificateRetriever, ISignedXmlFactory signedXmlFactory)
        {
            _certificateRetriever = certificateRetriever ?? throw new ArgumentNullException(nameof(certificateRetriever));
            _signedXmlFactory = signedXmlFactory ?? throw new ArgumentNullException(nameof(_signedXmlFactory));
        }

        public bool VerifyXMLSignature(XmlDocument xmlDocument, string referenceIdAttributeName)
        {
            X509Certificate2 signingCertificate = ExtractCertificate(xmlDocument);
            if (!_certificateRetriever.IsCertificateInTrustedStore(signingCertificate.Thumbprint))
                throw new SigningCertificateNotTrustedException($"Certificate with thumbprint {signingCertificate.Thumbprint} was not found in trusted store");

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature", Constants.XMLNS_XMLDSIG);
            SignedXml signedXml = _signedXmlFactory.CreateSignedXml(xmlDocument, referenceIdAttributeName);
            signedXml.LoadXml((XmlElement)nodeList[0]);

            X509Certificate2 trustedCertificate = _certificateRetriever.GetCertificateFromTrustedStore(signingCertificate.Thumbprint);
            return signedXml.CheckSignature(trustedCertificate, verifySignatureOnly: true);
        }

        public bool VerifyXMLSignature(XmlDocument xmlDocument)
        {
            return VerifyXMLSignature(xmlDocument, referenceIdAttributeName: null);
        }

        public bool VerifyXMLSignature(string xml, string referenceIdAttributeName)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            return VerifyXMLSignature(xmlDocument, referenceIdAttributeName);
        }

        public bool VerifyXMLSignature(string xml)
        {
            return VerifyXMLSignature(xml, referenceIdAttributeName: null);
        }

        private X509Certificate2 ExtractCertificate(XmlDocument xmlDocument)
        {
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("X509Certificate", Constants.XMLNS_XMLDSIG);
            byte[] certificateByes = System.Text.Encoding.UTF8.GetBytes(nodeList[0].InnerText);
            X509Certificate2 cert = new X509Certificate2(certificateByes);
            return cert;
        }
    }
}
