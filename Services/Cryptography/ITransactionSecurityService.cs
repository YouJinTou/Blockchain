using Models;
using Secp256k1;

namespace Services.Cryptography
{
    public interface ITransactionSecurityService
    {
        SignedMessage GetTransactionSignature(Transaction transaction, string privateKey);

        bool TransactionVerified(SignedMessage signedTransaction);
    }
}
