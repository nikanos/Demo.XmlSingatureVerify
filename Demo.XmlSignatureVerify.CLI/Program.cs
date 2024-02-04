using CommandLine;
using Demo.XmlSignatureVerify.Lib;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Demo.XmlSignatureVerify.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }

        static void RunOptions(Options opts)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(opts.InputFile);

                var signedXmlFactory = new SignedXmlFactory();
                var certificateRetriever = new CertificateRetriever();

                var verifier = new XmlSignatureVerifier(certificateRetriever: certificateRetriever, signedXmlFactory: signedXmlFactory);
                bool result;
                if (string.IsNullOrEmpty(opts.ReferenceIdAttributeName))
                    result = verifier.VerifyXMLSignature(xmlDoc);
                else
                    result = verifier.VerifyXMLSignature(xmlDoc, opts.ReferenceIdAttributeName);

                if (result)
                    Console.WriteLine("XML Signature verification success");
                else
                    Console.WriteLine("XML Signature verification failure");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
        }
    }
}
