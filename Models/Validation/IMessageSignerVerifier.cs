using Org.BouncyCastle.Crypto.Parameters;

namespace Models.Validation
{
    public interface IMessageSignerVerifier
    {
        byte[] GetMessageSignature(string privateKey, string message);

        bool MessageVerified(
            byte[] signature, ECPublicKeyParameters publicKeyParams, string message);

        ECPublicKeyParameters GetPublicKeyParams(string privateKey);
    }
}
