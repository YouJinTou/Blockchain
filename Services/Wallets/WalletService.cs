using AutoMapper;
using Models;
using Models.Web.Wallets;
using Services.Cryptography;
using Services.Nodes;

namespace Services.Wallets
{
    public class WalletService : IWalletService
    {
        private INodeService nodeService;
        private ITransactionSecurityService securityService;

        public WalletService(
            INodeService nodeService, 
            ITransactionSecurityService securityService)
        {
            this.nodeService = nodeService;
            this.securityService = securityService;
        }

        public WalletCredentials CreateWallet(string password)
        {
            var wallet = new Wallet(password);
            var credentials = new WalletCredentials
            {
                Address = wallet.Address,
                PrivateKey = wallet.PrivateKey,
                PublicKey = wallet.PublicKey
            };

            this.nodeService.RegisterAddress(
                new Models.Web.Users.RegisterUserModel
                {
                    Address = credentials.Address,
                    Balance = 0.0m
                });

            return credentials;
        }

        public AddressHistory GetAddressHistory(string address)
        {
            return this.nodeService.GetAddressHistory(address);
        }

        public void SendTransaction(SendTransactionModel model)
        {
            var transactionBuffer = Mapper.Map<SendTransactionModel, Transaction>(model);
            var signature = this.securityService.GetTransactionSignature(
               transactionBuffer, model.PrivateKey);
            var transaction = new Transaction(
                model.From, 
                model.To, 
                model.Amount,
                transactionBuffer.PublicKey, 
                signature.Signature);

            this.nodeService.ReceiveTransaction(transaction, signature);
        }
    }
}
