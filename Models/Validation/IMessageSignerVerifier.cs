using Secp256k1;

namespace Models.Validation
{
    public interface IMessageSignerVerifier
    {
        SignedMessage GetMessageSignature(string privateKey, string message);

        bool MessageVerified(SignedMessage message, string publicKey);
    }
}
