﻿using Models.Validation;
using System;
using Models;
using Models.Web.Nodes;
using Models.Web.Users;
using Services.Generation;
using AutoMapper;
using Models.Web.Wallets;
using System.Linq;
using Secp256k1;
using Microsoft.Extensions.Options;
using Models.Web.Settings;

namespace Services.Nodes
{
    public class NodeService : INodeService
    {
        private Node node;

        public NodeService(
            IBlockGenerator blockGenerator,
            IBlockchainValidator chainValidator,
            ITransactionValidator transactionValidator,
            IOptions<FaucetSettings> faucetSettings)
        {
            this.node = new Node(
                "Outcrop",
                new Uri("http://localhost:53633/"),
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
            return Mapper.Map<Node, Node>(this.node);
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
