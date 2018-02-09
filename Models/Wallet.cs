using Secp256k1;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Models
{
    public class Wallet
    {
        private const string CoinSeed = "Coin seed";

        private string privateKey;
        private string address;
        private decimal amount;

        public Wallet(string password)
        {
            this.GeneratePrivateKey(password);

            this.GenerateAddress();
        }

        public string PrivateKey => this.privateKey;

        public string Address => this.address;

        public decimal Amount => this.amount;

        private void GeneratePrivateKey(string password)
        {
            var hmacSha512 = new HMACSHA512(Encoding.Unicode.GetBytes(CoinSeed));
            var hash = hmacSha512.ComputeHash(Encoding.Unicode.GetBytes(password));
            var leftSequence = new byte[hash.Length / 2];

            Array.Copy(hash, 0, leftSequence, 0, hash.Length / 2);

            var hashBuilder = new StringBuilder();

            foreach (var hashByte in leftSequence)
            {
                hashBuilder.Append(hashByte.ToString("x"));
            }

            this.privateKey = hashBuilder.ToString();
        }

        private void GenerateAddress()
        {
            var privateKey = Hex.HexToBigInteger(this.privateKey);
            var publicKey = Secp256k1.Secp256k1.G.Multiply(privateKey);
            this.address = publicKey.GetBitcoinAddress(false);
        }
    }
}
