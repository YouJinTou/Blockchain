using Models;
using Models.Web.Nodes;
using Models.Web.Users;
using Models.Web.Wallets;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace Services.Nodes
{
    public interface INodeService
    {
        Node GetNode();

        ICollection<Block> GetChain();

        void AddPeer(AddPeerModel model);

        void RegisterAddress(RegisterUserModel model);

        Transaction GetTransaction(string hash);

        void ReceiveBlock(Block block);

        decimal GetAddressBalance(string address);

        AddressHistory GetAddressHistory(string address);

        void ReceiveTransaction(
            Transaction model, byte[] signature, ECPublicKeyParameters publicKeyParams);
    }
}
