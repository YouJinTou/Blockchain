﻿using System.Collections.Generic;

namespace Models.Validation
{
    public interface ITransactionValidator
    {
        void ValidateTransaction(
            Transaction transaction, IDictionary<string, decimal> balances);
    }
}
