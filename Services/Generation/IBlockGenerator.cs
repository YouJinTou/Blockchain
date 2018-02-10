using Models;
using System.Collections.Generic;

namespace Services.Generation
{
    public interface IBlockGenerator
    {
        Block GenerateBlock(ICollection<Transaction> transactions);
    }
}
