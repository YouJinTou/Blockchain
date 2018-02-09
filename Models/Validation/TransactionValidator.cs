using Models.Hashing;
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

        public bool TransactionIsValid(
            Transaction transaction, IDictionary<Address, decimal> balances)
        {
            if (transaction.Amount <= 0.0m)
            {
                return false;
            }

            if (transaction.ToString() != this.hasher.GetHash(transaction.GetMetadataString()))
            {
                return false;
            }

            if (balances[transaction.From] < transaction.Amount)
            {
                return false;
            }

            return true;
        }
    }
}
