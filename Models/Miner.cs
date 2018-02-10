using Models.Mining;

namespace Models
{
    public class Miner
    {
        private string address;
        private IConsensusStrategy consensusStrategy;

        public Miner(string address, IConsensusStrategy consensusStrategy)
        {
            this.address = address;
            this.consensusStrategy = consensusStrategy;
        }

        public Block MineBlock(Node node)
        {
            return this.consensusStrategy.GetBlock(new ChainStats
            {
                Difficulty = node.Difficulty,
                LastBlockHash = node.LastBlock.Hash,
                LastBlockId = node.LastBlock.Id,
                MinerAddress = this.address,
                PendingTransactions = node.PendingTransactions
            });
        }
    }
}
