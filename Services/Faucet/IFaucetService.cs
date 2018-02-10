using Models.Web.Faucet;

namespace Services.Faucet
{
    public interface IFaucetService
    {
        decimal GetBalance();

        void SendFunds(FaucetSendModel model);
    }
}
