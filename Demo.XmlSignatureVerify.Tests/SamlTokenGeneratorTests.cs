using Demo.XmlSignatureVerify.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.XmlSignatureVerify.Tests
{
    [TestClass]
    public class SamlTokenGeneratorTests
    {
        [TestMethod]
        [ExpectedException(typeof(System.Security.Cryptography.CryptographicException))]
        public void SamlTokenGenerator_InitializeWithWrongCertificatePassword_Throws()
        {
            var tokenGenerator = new SamlTokenGenerator(Resources.DEMO_XML_SIGN, "dummy_nonexisting_password");
        }

        [TestMethod]
        public void SamlTokenGenerator_InitializeWithCorrectCertificatePassword_Suceeds()
        {
            var tokenGenerator = new SamlTokenGenerator(Resources.DEMO_XML_SIGN, Constants.CERTIFICATE_PASSWORD);
        }

        [TestMethod]
        public void SamlTokenGenerator_GeneratoToken_SuceedsAndReturnsAToken()
        {
            var tokenGenerator = new SamlTokenGenerator(Resources.DEMO_XML_SIGN, Constants.CERTIFICATE_PASSWORD);
            string token = tokenGenerator.GenerateToken();
            Assert.IsNotNull(token);
        }
    }
}
