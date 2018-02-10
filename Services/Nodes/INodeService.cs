using Models;
using Models.Web.Nodes;
using Models.Web.Users;
using Models.Web.Wallets;
using Secp256k1;

namespace Services.Nodes
{
    public interface INodeService
    {
        Node GetNode();

        void AddPeer(AddPeerModel model);

        void RegisterAddress(RegisterUserModel model);

        Transaction GetTransaction(string hash);

        void ReceiveBlock(Block block);

        AddressHistory GetAddressHistory(string address);

        void ReceiveTransaction(Transaction model, SignedMessage signature);
    }
}
