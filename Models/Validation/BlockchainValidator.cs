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
            if (this.ShouldRequestChain(
                thisPeer.Blockchain.Last?.Value, otherPeer.Blockchain.Last?.Value))
            {
                return this.PeerChainValid(thisPeer.Blockchain, otherPeer.Blockchain);
            }

            return false;
        }

        public bool BlockIsValid(Block currentTailBlock, Block newBlock)
        {
            if (this.IsGenesisBlock(currentTailBlock, newBlock))
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

            if (currentTailBlock.Id != newBlock.Id - 1)
            {
                return false;
            }

            if (currentTailBlock.Hash != newBlock.PreviousHash)
            {
                return false;
            }

            return this.ProofOfWorkValid(newBlock);
        }

        private bool ShouldRequestChain(Block currentTailBlock, Block newBlock)
        {
            if (currentTailBlock == null && newBlock != null)
            {
                return true;
            }

            if (currentTailBlock == null)
            {
                return false;
            }

            return (currentTailBlock.Id < newBlock.Id);
        }

        private bool PeerChainValid(LinkedList<Block> thisChain, LinkedList<Block> otherChain)
        {
            if (this.IsGenesisBlock(thisChain.First?.Value, otherChain.Last?.Value))
            {
                return true;
            }

            if (otherChain.First.Value.Hash != thisChain.First?.Value.Hash)
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

        private bool IsGenesisBlock(Block currentTailBlock, Block newBlock)
        {
            return (currentTailBlock == null && newBlock != null);
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
