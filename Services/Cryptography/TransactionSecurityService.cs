using Models;
using Models.Validation;
using Org.BouncyCastle.Crypto.Parameters;
using System.Linq;

namespace Services.Cryptography
{
    public class TransactionSecurityService : ITransactionSecurityService
    {
        private IMessageSignerVerifier messageSignerVerifier;

        public TransactionSecurityService(IMessageSignerVerifier messageSignerVerifier)
        {
            this.messageSignerVerifier = messageSignerVerifier;
        }

        public ECPublicKeyParameters GetPublicKeyParams(string privateKey)
        {
            return this.messageSignerVerifier.GetPublicKeyParams(privateKey);
        }

        public byte[] GetTransactionSignature(Transaction transaction, string privateKey)
        {
            return this.messageSignerVerifier.GetMessageSignature(
                privateKey, transaction.GetMetadataString());
        }

        public string GetTransactionSignatureString(Transaction transaction, string privateKey)
        {
            var signature = this.GetTransactionSignature(transaction, privateKey);

            return string.Join(string.Empty, signature.Select(b => b.ToString("x")));
        }

        public bool TransactionVerified(
            byte[] signature, ECPublicKeyParameters publicKeyParams, string message)
        {
            return this.messageSignerVerifier.MessageVerified(signature, publicKeyParams, message);
        }
    }
}
