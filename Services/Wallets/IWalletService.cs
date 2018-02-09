namespace Services.Wallets
{
    public interface IWalletService
    {
        WalletCredentials CreateWallet(string password);
    }
}
