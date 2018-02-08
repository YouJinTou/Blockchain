using System.Security.Cryptography;
using System.Text;

namespace Models.Hashing
{
    public class Sha256Hasher : IHasher
    {
        public string GetHash(string seed)
        {
            var metadataBytes = Encoding.Unicode.GetBytes(seed);
            var hasher = new SHA256Managed();
            var hashBytes = hasher.ComputeHash(metadataBytes);
            var hashBuilder = new StringBuilder();

            foreach (var hashByte in hashBytes)
            {
                hashBuilder.Append(hashByte.ToString("x"));
            }

            return hashBuilder.ToString();
        }
    }
}
