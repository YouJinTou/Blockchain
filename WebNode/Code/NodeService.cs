using Models.Validation;
using WebNode.ApiModels.Users;
using System;
using Models;
using AutoMapper;
using WebNode.ApiModels.Nodes;

namespace WebNode.Code
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
            return this.node;
        }

        public void AddPeer(AddPeerModel model)
        {
            this.node.AddPeer(Mapper.Map<AddPeerModel, Node>(model));
        }

        public void RegisterAddress(RegisterUserModel model)
        {
            this.node.RegisterAddress(model.Address, model.Balance);
        }

        public void ReceiveTransaction(TransactionModel model)
        {
            this.node.ReceiveTransaction(Mapper.Map<TransactionModel, Transaction>(model));
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
    }
}
