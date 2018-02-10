﻿using Models;
using Services.Nodes;

namespace Services.Wallets
{
    public class WalletService : IWalletService
    {
        private INodeService nodeService;

        public WalletService(INodeService nodeService)
        {
            this.nodeService = nodeService;
        }

        public WalletCredentials CreateWallet(string password)
        {
            var wallet = new Wallet(password);
            var credentials = new WalletCredentials
            {
                Address = wallet.Address,
                PrivateKey = wallet.PrivateKey
            };

            this.nodeService.RegisterAddress(
                new Models.Web.Users.RegisterUserModel
                {
                    Address = credentials.Address,
                    Balance = 0.0m
                });

            return credentials;
        }
    }
}