using Models.Web.Faucet;

namespace Services.Faucet
{
    public interface IFaucetService
    {
        void SendFunds(FaucetSendModel model);
    }
}
