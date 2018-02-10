﻿using Models;
using Models.Validation;
using Secp256k1;

namespace Services.Cryptography
{
    public class TransactionSecurityService : ITransactionSecurityService
    {
        private IMessageSignerVerifier messageSignerVerifier;

        public TransactionSecurityService(IMessageSignerVerifier messageSignerVerifier)
        {
            this.messageSignerVerifier = messageSignerVerifier;
        }

        public SignedMessage GetTransactionSignature(Transaction transaction, string privateKey)
        {
            return this.messageSignerVerifier.GetMessageSignature(
                privateKey, transaction.GetMetadataString());
        }

        public bool TransactionVerified(SignedMessage signedTransaction, string publicKey)
        {
            return this.messageSignerVerifier.MessageVerified(signedTransaction, publicKey);
        }
    }
}
