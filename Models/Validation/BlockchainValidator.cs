using System.Collections.Generic;

namespace Models.Validation
{
    public class BlockchainValidator : IBlockchainValidator
    {
        public bool ShouldUpdateChain(Node thisPeer, Node otherPeer)
        {
            if (!this.BlockIsValid(
                thisPeer.Blockchain.Last.Value, otherPeer.Blockchain.Last.Value))
            {
                return false;
            }

            if (thisPeer.Blockchain.Last.Value.Id < otherPeer.Blockchain.Last.Value.Id)
            {
                if (this.PeerChainValid(thisPeer.Blockchain, otherPeer.Blockchain))
                {
                    return true;
                }
            }

            return false;
        }

        public bool BlockIsValid(Block currentTailBlock, Block newBlock)
        {
            if (currentTailBlock.Id >= newBlock.Id)
            {
                return false;
            }

            if (currentTailBlock.Hash != newBlock.PreviousHash)
            {
                return false;
            }

            var actualLeadingZeros = newBlock.Hash.Substring(0, (int)newBlock.Difficulty + 1);
            var expectedLeadingZeros = new string('0', (int)newBlock.Difficulty);

            return (actualLeadingZeros == expectedLeadingZeros);
        }

        private bool PeerChainValid(LinkedList<Block> thisChain, LinkedList<Block> otherChain)
        {
            if (otherChain.First.Value.Hash != thisChain.First.Value.Hash)
            {
                return false;
            }

            var currentBlock = otherChain.First;

            while (currentBlock.Next != null)
            {
                if (!this.BlockIsValid(currentBlock.Value, currentBlock.Next.Value))
                {
                    return false;
                }

                currentBlock = currentBlock.Next;
            }

            return true;
        }
    }
}
