using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Models
{
    // https://github.com/blockchain-dev-camp/blockchain-wallet/blob/master/BlockchainWallet/BlockchainWallet.Services/AddressService.cs
    public class Wallet
    {
        private const string CoinSeed = "Coin seed";
        private const string SignerType = "SHA-256withECDSA";
        private static readonly X9ECParameters CurveParams = SecNamedCurves.GetByName("secp256k1");
        private static readonly ECDomainParameters DomainParams = 
            new ECDomainParameters(CurveParams.Curve, CurveParams.G, CurveParams.N, CurveParams.H);

        private string privateKey;
        private string publicKey;
        private string address;

        public Wallet(string password)
        {
            this.privateKey = this.GetPrivateKey(password);
            this.publicKey = GetPublicKey(privateKey);
            this.address = this.GetAddress(this.publicKey);
        }

        public string PrivateKey => this.privateKey;

        public string PublicKey => this.publicKey;

        public string Address => this.address;

        public static string GetPublicKey(string privateKey)
        {
            var publicKeyParams = GetPublicKeyParams(privateKey);

            return BytesToHex(publicKeyParams.Q.GetEncoded());
        }

        public static ECPublicKeyParameters GetPublicKeyParams(string privateKey)
        {
            var seed = new BigInteger(Encoding.UTF8.GetBytes(privateKey));
            var publicKeyPair = DomainParams.G.Multiply(seed);

            return new ECPublicKeyParameters(publicKeyPair, DomainParams);
        }

        public static byte[] GetTransactionSignature(string message, byte[] privateKey)
        {
            var privateKeyBigInt = new BigInteger(privateKey);
            var privateKeyParams = new ECPrivateKeyParameters(privateKeyBigInt, DomainParams);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var signer = SignerUtilities.GetSigner(SignerType);

            signer.Init(true, privateKeyParams);

            signer.BlockUpdate(messageBytes, 0, messageBytes.Length);

            var signature = signer.GenerateSignature();

            return signature;
        }

        public static bool SignatureValid(
            ECPublicKeyParameters publicKeyParams, byte[] signature, string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var signer = SignerUtilities.GetSigner(SignerType);

            signer.Init(false, publicKeyParams);

            signer.BlockUpdate(messageBytes, 0, messageBytes.Length);

            return signer.VerifySignature(signature);
        }

        private string GetPrivateKey(string password)
        {
            var hmacSha512 = new HMACSHA512(Encoding.Unicode.GetBytes(CoinSeed));
            var hash = hmacSha512.ComputeHash(Encoding.Unicode.GetBytes(password));
            var keyLength = (hash.Length / 2) + 1;
            var leftSequence = new byte[keyLength];

            Array.Copy(hash, 0, leftSequence, 0, keyLength);

            return BytesToHex(leftSequence);
        }

        private string GetAddress(string publicKey)
        {
            using (var ripeMd160 = new SshNet.Security.Cryptography.RIPEMD160())
            {
                var addressRipe = ripeMd160.ComputeHash(Encoding.Unicode.GetBytes(publicKey));

                return BytesToHex(addressRipe);
            }
        }

        private static string BytesToHex(byte[] bytes)
        {
            return string.Join(string.Empty, bytes.Select(h => h.ToString("x")));
        }
    }
}
