using CommandLine;

namespace Demo.XmlSignatureVerify.CLI
{
    class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input signed XML file.")]
        public string InputFile { get; set; }

        [Option('n', "name", Required = false, HelpText = "Reference ID attribute name")]
        public string ReferenceIdAttributeName { get; set; }

    }
}
