using Secp256k1;
using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Models
{
    public class Wallet
    {
        private const string CoinSeed = "Coin seed";

        private string privateKey;
        private string publicKey;
        private Secp256k1.ECPoint publicKeyPair;
        private string address;

        public Wallet(string password)
        {
            this.GeneratePrivateKey(password);

            this.GeneratePublicKey();

            this.GenerateAddress();
        }

        public string PrivateKey => this.privateKey;

        public string PublicKey => this.publicKey;

        public string Address => this.address;

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

        private void GeneratePublicKey()
        {
            var privateKey = Hex.HexToBigInteger(this.privateKey);
            this.publicKeyPair = Secp256k1.Secp256k1.G.Multiply(privateKey);
            this.publicKey = this.CompressEcPoint(publicKeyPair);
        }

        private void GenerateAddress()
        {
            this.address = this.publicKeyPair.GetBitcoinAddress(false);
        }

        private string CompressEcPoint(Secp256k1.ECPoint point)
        {
            return point.X.ToString("x2") + Convert.ToInt32(!point.X.TestBit(0));
        }
    }
}
