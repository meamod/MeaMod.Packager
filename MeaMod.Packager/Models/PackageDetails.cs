
namespace MeaMod.Packager.Models
{
    public class PackageDetails
    {
        public string? FileName { get; set; }
        public string? Version { get; set; }
        public long? Size { get; set; }
        public PackageHashes? Hashes { get; set; }

    }
}
