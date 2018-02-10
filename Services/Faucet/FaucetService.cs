using AutoMapper;
using Microsoft.Extensions.Options;
using Models;
using Models.Web.Faucet;
using Models.Web.Settings;
using Services.Cryptography;
using Services.Nodes;

namespace Services.Faucet
{
    public class FaucetService : IFaucetService
    {
        private INodeService nodeService;
        private ITransactionSecurityService securityService;
        private IOptions<FaucetSettings> settings;

        public FaucetService(
            INodeService nodeService, 
            ITransactionSecurityService securityService, 
            IOptions<FaucetSettings> settings)
        {
            this.nodeService = nodeService;
            this.securityService = securityService;
            this.settings = settings;
        }

        public decimal GetBalance()
        {
            return this.nodeService.GetAddressBalance(this.settings.Value.Address);
        }

        public void SendFunds(FaucetSendModel model)
        {
            model.From = this.settings.Value.Address;
            model.PrivateKey = this.settings.Value.PrivateKey;
            var transactionBuffer = Mapper.Map<FaucetSendModel, Transaction>(model);
            var signature = this.securityService.GetTransactionSignature(
               transactionBuffer, this.settings.Value.PrivateKey);
            var transaction = new Transaction(
                this.settings.Value.Address,
                model.To,
                model.Amount,
                transactionBuffer.PublicKey,
                signature.Signature);

            this.nodeService.ReceiveTransaction(transaction, signature);
        }
    }
}
