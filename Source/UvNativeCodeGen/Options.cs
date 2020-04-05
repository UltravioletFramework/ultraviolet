using System;
using CommandLine;

namespace UvNativeCodeGen
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "The path to the XML file to process.")]
        public String InputFile { get; set; }
    }
}
