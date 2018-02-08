using System.Collections.Generic;

namespace Models.Validation
{
    public interface IBlockValidator
    {
        bool BlockIsValid(
            IEnumerable<Transaction> pendingTransactions, 
            Block currentTailBlock, 
            Block newBlock);
    }
}
