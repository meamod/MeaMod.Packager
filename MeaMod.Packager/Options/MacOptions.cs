using CommandLine;

namespace MeaMod.Packager.Options
{
    internal class MacOptions : Options
    {

        [Option('v', "source-version", Required = true, HelpText = "Set the version in package details JSON file")]
        public override string? SourceVersion { get; set; }
    }
}
