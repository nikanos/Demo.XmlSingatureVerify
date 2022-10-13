using System;
using System.Runtime.Serialization;

namespace Demo.XmlSignatureVerify.Lib.Exceptions
{
    [Serializable]
    public class SigningCertificateNotTrustedException : Exception
    {
        public SigningCertificateNotTrustedException()
        {
        }

        public SigningCertificateNotTrustedException(string message) : base(message)
        {
        }

        public SigningCertificateNotTrustedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SigningCertificateNotTrustedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}