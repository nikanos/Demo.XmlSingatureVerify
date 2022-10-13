using System.Security.Cryptography.Xml;
using System.Xml;

namespace Demo.XmlSignatureVerify.Lib
{
    public interface ISignedXmlFactory
    {
        SignedXml CreateSignedXml(XmlDocument xmlDocument);
        SignedXml CreateSignedXml(XmlDocument xmlDocument, string referenceIdAttributeName);

    }
}
