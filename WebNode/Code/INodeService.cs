using Models;
using WebNode.ApiModels.Users;

namespace WebNode.Code
{
    public interface INodeService
    {
        Node GetNode();

        void RegisterAddress(RegisterUserModel model);

        void ReceiveTransaction(TransactionModel model);

        Transaction GetTransaction(string hash);

        void ReceiveBlock(Block block);
    }
}
