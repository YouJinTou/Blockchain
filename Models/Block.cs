using Models.Hashing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Block
    {
        private uint id;
        private IEnumerable<Transaction> transactions;
        private uint difficulty;
        private string previousHash;
        private Address minedBy;
        private ulong nonce;
        private DateTime minedOn;
        private string hash;

        public Block(
            uint id, 
            IEnumerable<Transaction> transactions,
            uint difficulty,
            string previousHash,
            Address minedBy,
            ulong nonce,
            IHasher hasher = null)
        {
            this.id = id;
            this.transactions = transactions;
            this.difficulty = difficulty;
            this.previousHash = previousHash;
            this.minedBy = minedBy;
            this.nonce = nonce;
            this.minedOn = DateTime.Now;

            this.CalculateHash(hasher ?? new Sha256Hasher());
        }

        public uint Id => this.id;

        public IEnumerable<Transaction> Transactions => this.transactions;

        public uint Difficulty => this.difficulty;

        public string Hash => this.hash;

        public string PreviousHash => this.previousHash;

        public Address MinedBy => this.minedBy;

        public ulong Nonce => this.nonce;

        public DateTime MinedOn => this.minedOn;

        private void CalculateHash(IHasher hasher)
        {
            this.hash = hasher.GetHash(this.GetMetadataString());
        }

        public string GetMetadataString()
        {
            var sb = new StringBuilder();

            sb.Append(this.difficulty.ToString());
            sb.Append(this.id.ToString());
            sb.Append(this.minedBy.ToString());
            sb.Append(this.minedOn.Ticks.ToString());
            sb.Append(this.nonce.ToString());
            sb.Append(this.previousHash.ToString());

            return sb.ToString();
        }
    }
}
