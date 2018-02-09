using System.Collections.Generic;

namespace Models.Validation
{
    public interface IBlockchainValidator
    {
        bool BlockIsValid(Block currentTailBlock, Block newBlock);

        bool ShouldUpdateChain(Node currentPeer, Node peerToCheckAgainst);
    }
}
