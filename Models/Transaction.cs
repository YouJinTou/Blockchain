using Models.Hashing;
using System;
using System.Text;

namespace Models
{
    public class Transaction
    {
        private string from;
        private string to;
        private decimal amount;
        private string signature;
        private DateTime createdOn;
        private string hash;

        public Transaction(
            string from, 
            string to, 
            decimal amount, 
            string signature, 
            IHasher hasher = null)
        {
            this.from = from;
            this.to = to;
            this.amount = amount;
            this.signature = signature;
            this.createdOn = DateTime.Now;

            this.CalculateHash(hasher ?? new Sha256Hasher());
        }

        public string From => this.from;

        public string To => this.to;

        public decimal Amount => this.amount;

        public string Signature => this.signature;

        public uint? BlockId { get; set; } 

        public DateTime CreatedOn => this.createdOn;

        public string Hash => this.hash;

        public string GetMetadataString()
        {
            var sb = new StringBuilder();

            sb.Append(this.from.ToString());
            sb.Append(this.to.ToString());
            sb.Append(this.amount.ToString());
            //sb.Append(this.signature.ToString());
            //sb.Append(this.createdOn.Ticks.ToString());

            return sb.ToString();
        }

        public override string ToString()
        {
            return this.hash;
        }

        private void CalculateHash(IHasher hasher)
        {
            this.hash = hasher.GetHash(this.GetMetadataString());
        }
    }
}
