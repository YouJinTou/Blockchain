using System.Linq;

namespace Models.Mining
{
    public class ProofOfWork : IConsensusStrategy
    {
        public Block GetBlock(ChainStats chainStats)
        {
            uint nonce = 0;
            var newBlockId = chainStats.LastBlockId + 1;
            var transactions = chainStats.PendingTransactions
                .Select(t => { t.BlockId = newBlockId; return t; })
                .ToList();

            while (true)
            {
                var block = new Block(
                    newBlockId,
                    transactions,
                    chainStats.Difficulty,
                    chainStats.LastBlockHash,
                    chainStats.MinerAddress,
                    nonce);
                var expectedLeadingString = new string('0', (int)chainStats.Difficulty);
                var actualLeadingString = block.Hash.Substring(0, (int)chainStats.Difficulty);

                if (expectedLeadingString.Equals(actualLeadingString))
                {
                    return block;
                }

                nonce++;
            }
        }
    }
}
