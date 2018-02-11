using Models;
using Org.BouncyCastle.Crypto.Parameters;

namespace Services.Cryptography
{
    public interface ITransactionSecurityService
    {
        byte[] GetTransactionSignature(Transaction transaction, string privateKey);

        string GetTransactionSignatureString(Transaction transaction, string privateKey);

        bool TransactionVerified(
            byte[] signature, ECPublicKeyParameters publicKeyParams, string message);

        ECPublicKeyParameters GetPublicKeyParams(string privateKey);
    }
}
