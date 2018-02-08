using System;
using System.Security.Cryptography;
using System.Text;

namespace Models
{
    public class Transaction
    {
        private Address from;
        private Address to;
        private decimal amount;
        private string signature;
        private uint blockId;
        private DateTime createdOn;
        private string hash;

        public Transaction(Address from, Address to, decimal amount, string signature, uint blockId)
        {
            this.from = from;
            this.to = to;
            this.amount = amount;
            this.signature = signature;
            this.blockId = blockId;
            this.createdOn = DateTime.Now;

            this.CalculateHash();
        }

        public Address From => this.from;

        public Address To => this.to;

        public decimal Amount => this.amount;

        public string Signature => this.signature;

        public DateTime CreatedOn => this.createdOn;

        public string Hash => this.hash;

        private void CalculateHash()
        {
            var metadataString = this.GetMetadataString();
            var metadataBytes = Encoding.Unicode.GetBytes(metadataString);
            var hasher = new SHA256Managed();
            var hashBytes = hasher.ComputeHash(metadataBytes);
            var hashBuilder = new StringBuilder();

            foreach (var hashByte in hashBytes)
            {
                hashBuilder.Append(hashByte.ToString("x"));
            }

            this.hash = hashBuilder.ToString();
        }

        private string GetMetadataString()
        {
            var sb = new StringBuilder();

            sb.Append(this.from.ToString());
            sb.Append(this.to.ToString());
            sb.Append(this.amount.ToString());
            sb.Append(this.signature.ToString());
            sb.Append(this.blockId.ToString());
            sb.Append(this.createdOn.Ticks.ToString());

            return sb.ToString();
        }
    }
}
