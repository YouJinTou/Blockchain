using System.Collections.Generic;

namespace Models.Mining
{
    public class ChainStats
    {
        public string MinerAddress { get; set; }

        public uint LastBlockId { get; set; }

        public ICollection<Transaction> PendingTransactions { get; set; }

        public uint Difficulty { get; set; }

        public string LastBlockHash { get; set; }
    }
}
