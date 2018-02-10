using Secp256k1;

namespace Services.Cryptography
{
    public class MessageSignerVerifier : Secp256k1.MessageSignerVerifier, IMessageSignerVerifier
    {
        public SignedMessage GetMessageSignature(string privateKey, string message)
        {
            return base.Sign(Hex.HexToBigInteger(privateKey), message);
        }

        public bool MessageVerified(SignedMessage message)
        {
            return base.Verify(message);
        }
    }
}
