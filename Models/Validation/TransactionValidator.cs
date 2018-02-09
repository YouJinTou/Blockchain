using Models.Hashing;
using System;
using System.Collections.Generic;

namespace Models.Validation
{
    public class TransactionValidator : ITransactionValidator
    {
        private IHasher hasher;

        public TransactionValidator(IHasher hasher)
        {
            this.hasher = hasher;
        }

        public void ValidateTransaction(
            Transaction transaction, IDictionary<Address, decimal> balances)
        {
            if (!balances.ContainsKey(transaction.From))
            {
                throw new ArgumentException($"Invalid sender {transaction.From.Id}.");
            }

            if (!balances.ContainsKey(transaction.To))
            {
                throw new ArgumentException($"Invalid recipient {transaction.To.Id}.");
            }

            if (transaction.Amount <= 0.0m)
            {
                throw new ArgumentException($"Invalid amount. Must be positive.");
            }

            if (transaction.ToString() != this.hasher.GetHash(transaction.GetMetadataString()))
            {
                throw new ArgumentException($"Invalid transaction hash.");
            }

            if (balances[transaction.From] < transaction.Amount)
            {
                throw new ArgumentException(
                    $"Tried to send {transaction.Amount}, but {transaction.From} " +
                    $"only has {balances[transaction.From]}.");
            }
        }
    }
}
