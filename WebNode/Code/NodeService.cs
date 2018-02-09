using Models.Validation;
using WebNode.ApiModels.Users;
using System;

namespace WebNode.Code
{
    public class NodeService : INodeService
    {
        private Models.Node node;

        public NodeService(
            IBlockchainValidator chainValidator, ITransactionValidator transactionValidator)
        {
            this.node = new Models.Node(
                "Outcrop",
                new Uri("http://localhost:53633/"),
                chainValidator,
                transactionValidator);
        }

        public void RegisterAddress(RegisterUserModel model)
        {
        }
    }
}
