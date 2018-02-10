using Models.Hashing;
using Secp256k1;
using System;
using System.Collections.Generic;

namespace Models.Validation
{
    public class TransactionValidator : ITransactionValidator
    {
        private IHasher hasher;
        private IMessageSignerVerifier signerVerifier;

        public TransactionValidator(IHasher hasher, IMessageSignerVerifier signerVerifier)
        {
            this.hasher = hasher;
            this.signerVerifier = signerVerifier;
        }

        public void ValidateTransaction(
            Transaction transaction, IDictionary<string, decimal> balances, SignedMessage signature)
        {
            if (!balances.ContainsKey(transaction.From))
            {
                throw new ArgumentException($"Invalid sender {transaction.From}.");
            }

            if (!balances.ContainsKey(transaction.To))
            {
                throw new ArgumentException($"Invalid recipient {transaction.To}.");
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

            if (!this.signerVerifier.MessageVerified(signature))
            {
                throw new ArgumentException($"Invalid transaction signature.");
            }
        }
    }
}
