using System.Collections.Generic;

namespace Models.Validation
{
    public interface ITransactionValidator
    {
        bool TransactionIsValid(
            Transaction transaction, IDictionary<Address, decimal> balances);
    }
}
