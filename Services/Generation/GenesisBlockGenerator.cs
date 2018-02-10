using Microsoft.Extensions.Options;
using Models;
using Models.Mining;
using Models.Web.Settings;
using System.Collections.Generic;

namespace Services.Generation
{
    public class GenesisBlockGenerator : IBlockGenerator
    {
        IConsensusStrategy consensusStrategy;
        IOptions<FaucetSettings> faucetSettings;

        public GenesisBlockGenerator(
            IConsensusStrategy consensusStrategy, IOptions<FaucetSettings> faucetSettings)
        {
            this.consensusStrategy = consensusStrategy;
            this.faucetSettings = faucetSettings;
        }

        public Block GenerateBlock(ICollection<Transaction> transactions)
        {
            var block = this.consensusStrategy.GetBlock(new ChainStats
            {
                MinerAddress = this.faucetSettings.Value.Address,
                Difficulty = 1,
                LastBlockHash = string.Empty,
                LastBlockId = 0,
                PendingTransactions = transactions
            });

            return block;
        }
    }
}
