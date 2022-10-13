using Demo.XmlSignatureVerify.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.XmlSignatureVerify.Tests
{
    [TestClass]
    public class XmlSignatureVerifierTests
    {
        [TestMethod]
        public void TestMethodCustomers()
        {
            string xml = Resources.customers_signed;
            Assert.IsNotNull(xml);

            var verifier = new XmlSignatureVerifier(new CertificateRequester(), new SignedXmlFactory());
            var result = verifier.VerifyXMLSignature(xml);
            Assert.IsTrue(result);
        }
    }
}
