using Microsoft.Extensions.Options;
using Models;
using Models.Web.Settings;
using System.Collections.Generic;

namespace Services.Generation
{
    public class GenesisBlockGenerator : IBlockGenerator
    {
        IOptions<FaucetSettings> faucetSettings;

        public GenesisBlockGenerator(IOptions<FaucetSettings> faucetSettings)
        {
            this.faucetSettings = faucetSettings;
        }

        public Block GenerateBlock(ICollection<Transaction> transactions)
        {
            return new Block(
                0, transactions, 0, "FAUCET", this.faucetSettings.Value.Address, 0);
        }
    }
}
