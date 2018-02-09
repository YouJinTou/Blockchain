namespace Secp256k1
{
    public class RIPEMD160
    {
        private static readonly SshNet.Security.Cryptography.RIPEMD160 ripemd160 = new SshNet.Security.Cryptography.RIPEMD160();

        public static byte[] Hash(byte[] data)
        {
            return ripemd160.ComputeHash(data);
        }

        public static byte[] Hash(string hexData)
        {
            byte[] bytes = Hex.HexToBytes(hexData);
            return Hash(bytes);
        }
    }
}
