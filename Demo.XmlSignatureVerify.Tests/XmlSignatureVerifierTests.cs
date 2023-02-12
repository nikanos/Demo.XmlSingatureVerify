using Demo.XmlSignatureVerify.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.XmlSignatureVerify.Tests
{
    [TestClass]
    public class XmlSignatureVerifierTests
    {
        [TestMethod]
        public void XmlSignatureVerifier_VerifyXMLSignature_WithSignedCustomersXml_Succeeds()
        {
            string xml = Resources.customers_signed;
            Assert.IsNotNull(xml);

            var verifier = CreateXmlSignatureVerifierHelper();
            var result = verifier.VerifyXMLSignature(xml);
            Assert.IsTrue(result);
        }

        private XmlSignatureVerifier CreateXmlSignatureVerifierHelper(ICertificateRetriever certificateRetriever = null,
                                                                      ISignedXmlFactory signedXmlFactory = null)
        {
            if (certificateRetriever == null)
                certificateRetriever = new CertificateRetriever();
            if (signedXmlFactory == null)
                signedXmlFactory = new SignedXmlFactory();

            return new XmlSignatureVerifier(certificateRetriever: certificateRetriever,
                                            signedXmlFactory: signedXmlFactory);
        }
    }
}
