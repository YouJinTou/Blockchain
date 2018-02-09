using Models.Validation;
using WebNode.ApiModels.Users;
using System;
using Models;
using AutoMapper;

namespace WebNode.Code
{
    public class NodeService : INodeService
    {
        private Node node;

        public NodeService(
            IBlockchainValidator chainValidator, ITransactionValidator transactionValidator)
        {
            this.node = new Node(
                "Outcrop",
                new Uri("http://localhost:53633/"),
                chainValidator,
                transactionValidator);
        }

        public void RegisterAddress(RegisterUserModel model)
        {
            this.node.RegisterAddress(new Address(model.Address), model.Balance);
        }

        public void ReceiveTransaction(TransactionModel model)
        {
            this.node.ReceiveTransaction(Mapper.Map<TransactionModel, Transaction>(model));
        }
    }
}
