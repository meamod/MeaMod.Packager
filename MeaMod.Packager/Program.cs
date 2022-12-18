using MeaMod.Packager;
using MeaMod.Packager.Models;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("MeaMod Packager");
        Console.WriteLine("Version: " + GetVersion());

        PackageDetails details = new();

        if (args.Length > 0)
        {
            if (File.Exists(args[0]))
            {
                FileInfo file = new(args[0]);
                details.FileName = file.Name;
                details.Version = FileVersionInfo.GetVersionInfo(file.FullName).FileVersion;
                details.Size = file.Length;
                details.Hashes = Hashes.GetHashes(file.FullName);

                string json = JsonSerializer.Serialize(details);
                File.WriteAllText(file.DirectoryName + @"\" + file.Name + @".json", json);
            } else
            {
                Console.WriteLine("Executable not found");
            }
        }
        else
        {
            Console.WriteLine("Executable not specified");
        }
    }

    public static string GetVersion()

    {
        Version? version = Assembly.GetExecutingAssembly().GetName().Version;

        if (version is not null)
        {
            return version.ToString();
        }
        else {
            return string.Empty;
        }
    }
}