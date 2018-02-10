using System.Linq;
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

            return string.Join(string.Empty, hashBytes.Select(b => b.ToString("x")));
        }
    }
}
