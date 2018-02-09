using Models.Hashing;
using System.Linq;

namespace Models
{
    public class Miner
    {
        private Address address;
        private Node node;
        private IHasher hasher;

        public Miner(Address address, Node node, IHasher hasher)
        {
            this.address = address;
            this.node = node;
            this.hasher = hasher;
        }

        public void MineBlock()
        {
            uint nonce = 0;

            while (true)
            {
                var newBlockId = this.node.LastBlock.Id + 1;
                var transactions = this.node.PendingTransactions
                    .Select(t => { t.BlockId = newBlockId; return t; });
                var block = new Block(
                    newBlockId,
                    transactions, 
                    this.node.Difficulty, 
                    this.node.LastBlock.Hash, 
                    this.address, 
                    nonce);
                var expectedLeadingString = new string('0', (int)this.node.Difficulty);
                var actualLeadingString = block.Hash.Substring(0, (int)this.node.Difficulty);

                if (expectedLeadingString.Equals(actualLeadingString))
                {
                    nonce = 0;

                    this.node.ReceiveBlock(block);
                }

                nonce++;
            }
        }
    }
}
