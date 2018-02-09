using System.Linq;

namespace Models
{
    public class Miner
    {
        private string address;

        public Miner(string address)
        {
            this.address = address;
        }

        public Block MineBlock(Node node)
        {
            uint nonce = 0;
            var newBlockId = node.LastBlock.Id + 1;
            var transactions = node.PendingTransactions
                .Select(t => { t.BlockId = newBlockId; return t; });

            while (true)
            {
                var block = new Block(
                    newBlockId,
                    transactions, 
                    node.Difficulty, 
                    node.LastBlock.Hash, 
                    this.address, 
                    nonce);
                var expectedLeadingString = new string('0', (int)node.Difficulty);
                var actualLeadingString = block.Hash.Substring(0, (int)node.Difficulty);

                if (expectedLeadingString.Equals(actualLeadingString))
                {
                    return block;
                }

                nonce++;
            }
        }
    }
}
