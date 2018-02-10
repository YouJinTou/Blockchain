using Secp256k1;

namespace Services.Cryptography
{
    public interface IMessageSignerVerifier
    {
        SignedMessage GetMessageSignature(string privateKey, string message);

        bool MessageVerified(SignedMessage message);
    }
}
