using Models.Web.Wallets;

namespace Services.Wallets
{
    public interface IWalletService
    {
        WalletCredentials CreateWallet(string password);

        AddressHistory GetAddressHistory(string address);

        void SendTransaction(SendTransactionModel model);
    }
}
