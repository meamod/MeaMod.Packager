using CommandLine;
using MeaMod.Packager;
using MeaMod.Packager.Model;
using MeaMod.Packager.Options;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Count() == 0)
        {
            args = new string[] { "--help" };
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Parser.Default.ParseArguments<WindowsOptions>(args)
            .WithParsed(Run)
            .WithNotParsed(HandleParseError);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Parser.Default.ParseArguments<MacOptions>(args)
            .WithParsed(Run)
            .WithNotParsed(HandleParseError);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Parser.Default.ParseArguments<LinuxOptions>(args)
            .WithParsed(Run)
            .WithNotParsed(HandleParseError);
        }
        else
        {
            Parser.Default.ParseArguments<LinuxOptions>(args)
            .WithParsed(Run)
            .WithNotParsed(HandleParseError);
        }
    }

    private static void Run(Options opts)
    {
        Console.WriteLine("MeaMod Packager");
        Console.WriteLine("Version: " + GetVersion());

        PackageDetails details = new();

        if (File.Exists(opts.SourceFile))
        {
            FileInfo file = new(opts.SourceFile);
            details.FileName = file.Name;
            if (String.IsNullOrEmpty(opts.SourceVersion))
            {
                details.Version = FileVersionInfo.GetVersionInfo(file.FullName).FileVersion;
            }
            else
            {
                details.Version = opts.SourceVersion;
            }

            details.Size = file.Length;
            details.Date = DateOnly.FromDateTime(DateTime.UtcNow);
            details.Hashes = Hashes.GetHashes(file.FullName);

            var options = new JsonSerializerOptions(JsonSerializerDefaults.General);
            options.Converters.Add(new DateOnlyConverter());

            string json = JsonSerializer.Serialize(details, options);

            if (String.IsNullOrWhiteSpace(opts.OutputDirectory))
            {
                if (!String.IsNullOrWhiteSpace(file.DirectoryName)){
                    opts.OutputDirectory = file.DirectoryName;
                }
  
            }

            if (!String.IsNullOrWhiteSpace(opts.OutputDirectory))
            {
                string[] paths = { opts.OutputDirectory, string.IsNullOrEmpty(opts.OutputFile) ? file.Name + @".json" : opts.OutputFile };
                string fullPath = Path.Combine(paths);

                File.WriteAllText(fullPath, json);
            }
            else
            {
                Console.WriteLine("Executable directory not found");
            }
        }
        else
        {
            Console.WriteLine("Executable not found");
        }
    }

    private static void HandleParseError(IEnumerable<Error> errs)
    {
        errs.Output();
    }

    public static string GetVersion()

    {
        Version? version = Assembly.GetExecutingAssembly().GetName().Version;

        if (version is not null)
        {
            return version.ToString();
        }
        else
        {
            return string.Empty;
        }
    }
}