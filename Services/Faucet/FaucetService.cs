using AutoMapper;
using Microsoft.Extensions.Options;
using Models;
using Models.Web.Faucet;
using Models.Web.Settings;
using Services.Cryptography;
using Services.Nodes;
using System;

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
            var random = new Random();
            var amount = (decimal)(random.NextDouble() * 10);
            model.From = this.settings.Value.Address;
            model.PrivateKey = this.settings.Value.PrivateKey;
            model.Amount = amount;
            var transactionBuffer = Mapper.Map<FaucetSendModel, Transaction>(model);
            var signature = this.securityService.GetTransactionSignature(
               transactionBuffer, this.settings.Value.PrivateKey);
            var signatureString = this.securityService.GetTransactionSignatureString(
               transactionBuffer, model.PrivateKey);
            var transaction = new Transaction(
                this.settings.Value.Address,
                model.To,
                amount,
                transactionBuffer.PublicKey,
                signatureString);
            var publicKeyParams = this.securityService.GetPublicKeyParams(model.PrivateKey);

            this.nodeService.ReceiveTransaction(transaction, signature, publicKeyParams);
        }
    }
}
