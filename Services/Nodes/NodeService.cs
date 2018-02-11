using AutoMapper;
using Microsoft.Extensions.Options;
using Models;
using Models.Validation;
using Models.Web.Nodes;
using Models.Web.Settings;
using Models.Web.Users;
using Models.Web.Wallets;
using Secp256k1;
using Services.Generation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Nodes
{
    public class NodeService : INodeService
    {
        private Node node;

        public NodeService(
            IBlockGenerator blockGenerator,
            IBlockchainValidator chainValidator,
            ITransactionValidator transactionValidator,
            IOptions<NodeSettings> nodeSettings,
            IOptions<FaucetSettings> faucetSettings)
        {
            this.node = new Node(
                nodeSettings.Value.Name,
                new Uri(nodeSettings.Value.Address),
                chainValidator,
                transactionValidator);
            var transaction = new Transaction(
                faucetSettings.Value.Address,
                faucetSettings.Value.Address,
                faucetSettings.Value.Balance,
                faucetSettings.Value.PublicKey,
                null);

            this.node.RegisterAddress(faucetSettings.Value.Address, faucetSettings.Value.Balance);

            this.node.PendingTransactions.Add(transaction);

            this.node.ReceiveBlock(blockGenerator.GenerateBlock(this.node.PendingTransactions));
        }

        public Node GetNode()
        {
            return this.node;
        }

        public ICollection<Block> GetChain()
        {
            return this.node.Blockchain;
        }

        public void AddPeer(AddPeerModel model)
        {
            this.node.AddPeer(Mapper.Map<AddPeerModel, Node>(model));
        }

        public void RegisterAddress(RegisterUserModel model)
        {
            this.node.RegisterAddress(model.Address, model.Balance);
        }

        public Transaction GetTransaction(string hash)
        {
            foreach (var block in this.node.Blockchain)
            {
                foreach (var transaction in block.Transactions)
                {
                    if (transaction.Hash.Equals(hash))
                    {
                        return transaction;
                    }
                }
            }

            throw new ArgumentException($"Invalid hash {hash}.");
        }

        public void ReceiveBlock(Block block)
        {
            this.node.ReceiveBlock(block);
        }

        public decimal GetAddressBalance(string address)
        {
            if (!this.node.Balances.ContainsKey(address))
            {
                throw new ArgumentException($"Invalid address {address}");
            }

            return this.node.Balances[address];
        }

        public AddressHistory GetAddressHistory(string address)
        {
            if (!this.node.Balances.ContainsKey(address))
            {
                return new AddressHistory();
            }

            var inTransactions = this.node.Blockchain
                .SelectMany(b => b.Transactions)
                .Where(t => t.To.Equals(address))
                .ToList();
            var outTransactions = this.node.Blockchain
                .SelectMany(b => b.Transactions)
                .Where(t => t.From.Equals(address))
                .ToList();
            var history = new AddressHistory
            {
                Address = address,
                Balance = this.node.Balances[address],
                InTransactions = inTransactions,
                OutTransactions = outTransactions
            };

            return history;
        }

        public void ReceiveTransaction(Transaction transaction, SignedMessage signature)
        {
            this.node.ReceiveTransaction(transaction, signature);
        }
    }
}
