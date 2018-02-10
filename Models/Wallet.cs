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
            this.privateKey = this.GeneratePrivateKey(password);
            this.publicKey = this.GeneratePublicKey(this.privateKey);
            this.publicKeyPair = GeneratePublicKeyPair(this.privateKey);
            this.address = this.GenerateAddress(this.publicKeyPair);
        }

        public string PrivateKey => this.privateKey;

        public string PublicKey => this.publicKey;

        public string Address => this.address;

        public static string GetPublicKey(string privateKey)
        {
            return CompressEcPoint(GeneratePublicKeyPair(privateKey));
        }

        private string GeneratePrivateKey(string password)
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

            return hashBuilder.ToString();
        }

        private static Secp256k1.ECPoint GeneratePublicKeyPair(string privateKey)
        {
            var privateKeyBigInt = Hex.HexToBigInteger(privateKey);

            return Secp256k1.Secp256k1.G.Multiply(privateKeyBigInt);
        }

        private string GeneratePublicKey(string privateKey)
        {
            var privateKeyBigInt = Hex.HexToBigInteger(privateKey);
            var publicKeyPair = Secp256k1.Secp256k1.G.Multiply(privateKeyBigInt);

            return CompressEcPoint(publicKeyPair);
        }

        private string GenerateAddress(Secp256k1.ECPoint publicKeyPair)
        {
            return publicKeyPair.GetBitcoinAddress(false);
        }

        private static string CompressEcPoint(Secp256k1.ECPoint point)
        {
            return point.X.ToString("x2") + Convert.ToInt32(!point.X.TestBit(0));
        }
    }
}
