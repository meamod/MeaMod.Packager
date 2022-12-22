using MeaMod.Packager.Model;
using System.Security.Cryptography;

namespace MeaMod.Packager
{
    public class Hashes
    {
        public static PackageHashes GetHashes(string fileName)
        {
            PackageHashes hashes = new();
            using (var md5 = MD5.Create())
            using (var sha1 = SHA1.Create())
            using (var sha256 = SHA256.Create())
            using (var sha512 = SHA512.Create())
            using (var input = File.OpenRead(fileName))
            {
                byte[] buffer = new byte[8192];
                int bytesRead;
                while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    md5.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                    sha1.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                    sha256.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                    sha512.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                }
                // We have to call TransformFinalBlock, but we don't have any
                // more data - just provide 0 bytes.
                md5.TransformFinalBlock(buffer, 0, 0);
                sha1.TransformFinalBlock(buffer, 0, 0);
                sha256.TransformFinalBlock(buffer, 0, 0);
                sha512.TransformFinalBlock(buffer, 0, 0);

                if (md5.Hash != null)
                {
                    hashes.MD5 = BitConverter.ToString(md5.Hash).Replace("-", string.Empty).ToLowerInvariant();
                }

                if (sha1.Hash != null)
                {
                    hashes.SHA1 = BitConverter.ToString(sha1.Hash).Replace("-", string.Empty).ToLowerInvariant();
                }

                if (sha256.Hash != null)
                {
                    hashes.SHA256 = BitConverter.ToString(sha256.Hash).Replace("-", string.Empty).ToLowerInvariant();
                }

                if (sha512.Hash != null)
                {
                    hashes.SHA512 = BitConverter.ToString(sha512.Hash).Replace("-", string.Empty).ToLowerInvariant();
                }
            }

            return hashes;
        }

    }
}
