using Org.BouncyCastle.Crypto.Parameters;
using System.Text;

namespace Models.Validation
{
    public class MessageSignerVerifier : IMessageSignerVerifier
    {
        public byte[] GetMessageSignature(string privateKey, string message)
        {
            return Wallet.GetTransactionSignature(message, Encoding.UTF8.GetBytes(message));
        }

        public ECPublicKeyParameters GetPublicKeyParams(string privateKey)
        {
            return Wallet.GetPublicKeyParams(privateKey);
        }

        public bool MessageVerified(
            byte[] signature, ECPublicKeyParameters publicKeyParams, string message)
        {
            return Wallet.SignatureValid(publicKeyParams, signature, message);
        }
    }
}
