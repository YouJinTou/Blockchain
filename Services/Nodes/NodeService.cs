using Models.Validation;
using System;
using Models;
using Models.Web.Nodes;
using Models.Web.Users;
using Services.Generation;
using AutoMapper;
using Models.Web.Wallets;
using System.Linq;
using Secp256k1;

namespace Services.Nodes
{
    public class NodeService : INodeService
    {
        private Node node;

        public NodeService(
            IBlockGenerator blockGenerator,
            IBlockchainValidator chainValidator, 
            ITransactionValidator transactionValidator)
        {
            this.node = new Node(
                "Outcrop",
                new Uri("http://localhost:53633/"),
                chainValidator,
                transactionValidator);

            this.node.ReceiveBlock(blockGenerator.GenerateBlock());
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
