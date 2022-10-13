using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Demo.XmlSignatureVerify.Tests.Utils
{
    public class SamlTokenGenerator
    {
        private X509Certificate2 _signingCertificate;

        public SamlTokenGenerator(byte[] signingCertificateBytes, string password)
        {
            if (signingCertificateBytes == null)
                throw new ArgumentNullException(nameof(signingCertificateBytes));
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (password == string.Empty)
                throw new ArgumentException("Should not be empty", nameof(password));

            _signingCertificate = new X509Certificate2(signingCertificateBytes, password);
        }

        public SamlTokenGenerator(string signingCertificateFileName, string password)
        {
            if (signingCertificateFileName == null)
                throw new ArgumentNullException(nameof(signingCertificateFileName));
            if (signingCertificateFileName == string.Empty)
                throw new ArgumentException("Should not be empty", nameof(signingCertificateFileName));
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (password == string.Empty)
                throw new ArgumentException("Should not be empty", nameof(password));

            _signingCertificate = new X509Certificate2(signingCertificateFileName, password);
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            if (claims == null)
                throw new ArgumentNullException(nameof(claims));
            if (!claims.Any())
                throw new ArgumentException("Should contain a claim", nameof(claims));

            var descriptor = new SecurityTokenDescriptor();
            descriptor.TokenType = "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0";

            DateTime tokenIssueDate = DateTime.UtcNow;
            descriptor.Lifetime = new Lifetime(created: tokenIssueDate, expires: tokenIssueDate + new TimeSpan(hours: 8, minutes: 0, seconds: 0));

            descriptor.AppliesToAddress = "http://localhost/MyRelyingPartyApplication";
            descriptor.TokenIssuerName = "MyTokenIssuer";
            descriptor.Subject = new ClaimsIdentity(claims);

            X509SigningCredentials signingCredentials = new X509SigningCredentials(_signingCertificate);
            descriptor.SigningCredentials = signingCredentials;

            Saml2SecurityTokenHandler tokenHandler = new Saml2SecurityTokenHandler();
            Saml2SecurityToken token = tokenHandler.CreateToken(descriptor) as Saml2SecurityToken;
            using (var ms = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(ms))
                {
                    tokenHandler.WriteToken(xmlWriter, token);
                    xmlWriter.Flush();
                    ms.Position = 0;
                    const int DEFAULT_STREAM_BUFFER_SIZE = 1024;
                    //Memory stream will be closed by the XML Writer, so We need to tell stream reader to keep it open
                    using (StreamReader sr = new StreamReader(ms, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: DEFAULT_STREAM_BUFFER_SIZE, leaveOpen: true))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        public string GenerateToken()
        {
            var defaultClaims = new List<Claim>() { new Claim("User", "Nik"), new Claim("Permission", "Read") };
            return GenerateToken(claims: defaultClaims);
        }
    }
}
