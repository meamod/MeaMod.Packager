using CommandLine;

namespace MeaMod.Packager.Options
{
    internal class Options
    {
        [Option('s', "source-file", Required = true, HelpText = "Sets source file used to generate package details JSON file")]
        public string? SourceFile { get; set; }

        [Option('v', "source-version", Required = false, HelpText = "Manually set the version in package details JSON file")]
        public virtual string? SourceVersion { get; set; }

        [Option('o', "output-file", Required = false, HelpText = "Sets the output JSON file name")]
        public string? OutputFile { get; set; }

        [Option('d', "output-directory", Required = false, HelpText = "Sets the output directory for the JSON file")]
        public string? OutputDirectory { get; set; }

    }
}
