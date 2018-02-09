using Models;

namespace Services.Wallets
{
    public class WalletService : IWalletService
    {
        public WalletCredentials CreateWallet(string password)
        {
            var wallet = new Wallet(password);
            var credentials = new WalletCredentials
            {
                Address = wallet.Address,
                PrivateKey = wallet.PrivateKey
            };

            return credentials;
        }
    }
}
