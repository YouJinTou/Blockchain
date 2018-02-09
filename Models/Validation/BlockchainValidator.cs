using Models.Hashing;
using System.Collections.Generic;

namespace Models.Validation
{
    public class BlockchainValidator : IBlockchainValidator
    {
        private IHasher hasher;

        public BlockchainValidator(IHasher hasher)
        {
            this.hasher = hasher;
        }

        public bool ShouldUpdateChain(Node thisPeer, Node otherPeer)
        {
            if (!this.BlockIsValid(
                thisPeer.Blockchain.Last?.Value, otherPeer.Blockchain.Last?.Value))
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
            var isGenesisBlock = (currentTailBlock == null && newBlock != null);

            if (isGenesisBlock)
            {
                return this.GenesisBlockValid(newBlock);
            }

            if (newBlock == null)
            {
                return false;
            }

            if (currentTailBlock.Id >= newBlock.Id)
            {
                return false;
            }

            if (currentTailBlock.Hash != newBlock.PreviousHash)
            {
                return false;
            }

            return this.ProofOfWorkValid(newBlock);
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

        private bool GenesisBlockValid(Block newBlock)
        {
            return (this.hasher.GetHash(newBlock.GetMetadataString()) == newBlock.Hash);
        }

        private bool ProofOfWorkValid(Block newBlock)
        {
            var actualLeadingString = newBlock.Hash.Substring(0, (int)newBlock.Difficulty);
            var expectedLeadingString = new string('0', (int)newBlock.Difficulty);

            return actualLeadingString.Equals(expectedLeadingString);
        }
    }
}
