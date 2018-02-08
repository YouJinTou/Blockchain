﻿using System.Collections.Generic;
using System.Linq;

namespace Models.Validation
{
    public class BlockValidator : IBlockValidator
    {
        public bool BlockIsValid(
            IEnumerable<Transaction> pendingTransactions, 
            Block currentTailBlock, 
            Block newBlock)
        {
            if (currentTailBlock.Id != newBlock.Id - 1)
            {
                return false;
            }

            if (currentTailBlock.Hash != newBlock.PreviousHash)
            {
                return false;
            }

            if (!this.TransactionsExist(pendingTransactions, newBlock.Transactions))
            {
                return false;
            }

            if (currentTailBlock.Hash != newBlock.Hash)
            {
                return false;
            }

            return true;
        }

        private bool TransactionsExist(
            IEnumerable<Transaction> pendingTransactions, IEnumerable<Transaction> transactions)
        {
            return pendingTransactions
                .Select(pt => pt.Hash)
                .All(h => transactions.Any(t => t.Hash == h));
        }
    }
}