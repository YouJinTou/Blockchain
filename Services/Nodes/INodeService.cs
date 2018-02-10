using Models;
using Models.Web.Nodes;
using Models.Web.Users;
using Models.Web.Wallets;

namespace Services.Nodes
{
    public interface INodeService
    {
        Node GetNode();

        void AddPeer(AddPeerModel model);

        void RegisterAddress(RegisterUserModel model);

        void ReceiveTransaction(TransactionModel model);

        Transaction GetTransaction(string hash);

        void ReceiveBlock(Block block);

        AddressHistory GetAddressHistory(string address);
    }
}
